using Bagombo.AuthHandlers;
using Bagombo.Controllers;
using Bagombo.Data.Command.EFCoreCommandHandlers;
using Bagombo.Data.Query;
using Bagombo.Data.Query.EFCoreQueryHandlers;
using Bagombo.EFCore;
using Bagombo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using System;
using System.Threading.Tasks;

namespace Bagombo
{
  public class Startup
  {
    IConfiguration Configuration;
    private Container _container = new Container();

    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddEnvironmentVariables();

      if (env.IsDevelopment())
      { 
          builder.AddUserSecrets<Startup>();
      }

      Configuration = builder.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
      
      // This is for a mutli-tenant Environment, so the ConnectionString Env Var name can be set
      // In AppSettings.json
      var ConnectionStringConfigName = Configuration["ConnectionStringConfigName"];
      var ConnectionString = Configuration[$"{ConnectionStringConfigName}"];

      if (ConnectionString == null)
      {
        ConnectionString = Configuration["ConnectionString"];
        if (ConnectionString == null)
        {
          throw new System.Exception("Unable to determine the Connection String to the database.");
        }
      }

      services.Configure<BagomboSettings>(Configuration.GetSection("BagomboSettings"));

      services.AddSession();

      services.AddMvc().AddJsonOptions(options =>
      {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
      });

      services.AddDbContext<BlogDbContext>(options => {
        options.UseSqlServer(ConnectionString);
      });

      services.AddDbContext<ApplicationDbContext>(options => {
        options.UseSqlServer(ConnectionString);
      });

      services.AddIdentity<ApplicationUser, IdentityRole>(opts => {
        opts.User.RequireUniqueEmail = true;
      }).AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

      var authBuilder = services.AddAuthentication();

      var TwitterKey = Configuration[$"{Configuration["TwitterKeyConfigName"]}"];
      var TwitterSecret = Configuration[$"{Configuration["TwitterSecretConfigName"]}"];

      if (TwitterKey != null && TwitterSecret != null)
      {
        authBuilder.AddTwitter(opts =>
        {
          opts.ConsumerKey = TwitterKey;
          opts.ConsumerSecret = TwitterSecret;
        });
      }

      var FacebookAppId = Configuration[$"{Configuration["FacebookAppIdConfigName"]}"];
      var FacebookAppSecret = Configuration[$"{Configuration["FacebookAppSecretConfigName"]}"];

      if (FacebookAppId != null && FacebookAppSecret != null)
      {
        authBuilder.AddFacebook(opts =>
        {
          opts.AppId = FacebookAppId;
          opts.AppSecret = FacebookAppSecret;
        });
      }

      services.ConfigureApplicationCookie(opts =>
      {
        opts.Cookie.Expiration = TimeSpan.FromDays(14); 
        opts.ExpireTimeSpan = TimeSpan.FromDays(14);
        opts.Cookie.Name = "SecurityLogin";
      });

      services.AddAntiforgery(opts =>
      {
       opts.Cookie.Name = "SecurityAntiForgery";
      });

      services.AddAuthorization(opts =>
      {
        opts.AddPolicy("EditPolicy", policy =>
        {
          policy.Requirements.Add(new SameAuthorRequirement());
        });
      });

      services.AddScoped<IAuthorizationHandler>(p => new SimpleInjectorAuthorizationHandler(_container));

      IntegrateSimpleInjector(services);
    }

    public void IntegrateSimpleInjector(IServiceCollection services)
    {
      _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();


      _container.RegisterCollection<IAuthorizationHandler>(new Type[] { typeof(EditBlogPostAuthorizationHandler) });

      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(_container));
      services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(_container));

      services.EnableSimpleInjectorCrossWiring(_container);
      services.UseSimpleInjectorAspNetRequestScoping(_container);
      services.AddSimpleInjectorTagHelperActivation(_container);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {

      InitializeContainer(app);

      _container.Verify();

      // Able to move this after verify after discovering how to create the scoped instances correctly
      // See - https://github.com/aspnet/EntityFramework/issues/5096  and
      // https://github.com/simpleinjector/SimpleInjector/issues/398

      ApplicationDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();

      ApplicationDbContext.CreateAuthorRole(app.ApplicationServices).Wait();

      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddFile(Configuration.GetSection("Logging"));

      app.UseStaticFiles();

      app.UseSession();

      if (env.IsDevelopment())
      {
        app.UseStatusCodePages();
        app.UseDeveloperExceptionPage();
      }

      app.UseAuthentication();

      app.UseMvcWithDefaultRoute();

    }

    private void InitializeContainer(IApplicationBuilder app)
    {

      // Add application presentation components:
      _container.RegisterMvcControllers(app);
      _container.RegisterMvcViewComponents(app);

      // Cross-wire ASP.NET services (if any). For instance:
      _container.RegisterSingleton(app.ApplicationServices.GetService<ILoggerFactory>());

      _container.CrossWire<BlogDbContext>(app);
      _container.CrossWire<ApplicationDbContext>(app);
      _container.CrossWire<UserManager<ApplicationUser>>(app);
      _container.CrossWire<RoleManager<IdentityRole>>(app);
      _container.CrossWire<SignInManager<ApplicationUser>>(app);
      _container.CrossWire<IPasswordHasher<ApplicationUser>>(app);
      _container.CrossWire<IPasswordValidator<ApplicationUser>>(app);
      _container.CrossWire<IUserValidator<ApplicationUser>>(app);
      _container.CrossWire<IOptions<BagomboSettings>>(app);
      _container.CrossWire<ILogger<HomeController>>(app);
      _container.CrossWire<ILogger<AccountController>>(app);
      _container.CrossWire<ILogger<AdminController>>(app);
      _container.CrossWire<ILogger<AuthorController>>(app);
      _container.CrossWire<ILogger<MetaController>>(app);
      _container.CrossWire<IAuthorizationHandler>(app);
      _container.CrossWire<IAuthorizationService>(app);

      _container.AddEFQueries();
      _container.AddEFCommands();

      // NOTE: Do prevent cross-wired instances as much as possible.
      // See: https://simpleinjector.org/blog/2016/07/
    }
  }

  public sealed class SimpleInjectorAuthorizationHandler : IAuthorizationHandler
  {
    private readonly Container container;
    public SimpleInjectorAuthorizationHandler(Container container) { this.container = container; }

    public async Task HandleAsync(AuthorizationHandlerContext context)
    {
      foreach (var handler in this.container.GetAllInstances<IAuthorizationHandler>())
        await handler.HandleAsync(context);
    }

  }
}

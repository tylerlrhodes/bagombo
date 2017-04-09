using Bagombo.Data.Command.EFCoreCommandHandlers;
using Bagombo.Data.Query.EFCoreQueryHandlers;
using Bagombo.EFCore;
using Bagombo.Models;
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
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using System;

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

      services.AddMvc().AddJsonOptions(options =>
      {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
      });

      services.AddDbContext<BlogDbContext>(options =>
                  options.UseSqlServer(ConnectionString));

      services.AddIdentity<ApplicationUser, IdentityRole>(opts => {
        opts.User.RequireUniqueEmail = true;
      }).AddEntityFrameworkStores<BlogDbContext>()
        .AddDefaultTokenProviders();

      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(_container) );
      services.AddSingleton<IViewComponentActivator>( new SimpleInjectorViewComponentActivator(_container) );

      services.UseSimpleInjectorAspNetRequestScoping(_container);
      services.AddSimpleInjectorTagHelperActivation(_container);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      // This has to be called before Verify to avoid accessing a disposed object
      // Due to it's lifestyle beings scoped when it is cross-wired
      // Verify will dispose of scoped registrations after running
      BlogDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();
      BlogDbContext.CreateAuthorRole(app.ApplicationServices).Wait();

      InitializeContainer(app);

      _container.Verify();

      loggerFactory.AddConsole();
      loggerFactory.AddFile("Logs/ts-{Date}.txt");

      app.UseStaticFiles();

      if (env.IsDevelopment())
      {
        app.UseStatusCodePages();
        app.UseDeveloperExceptionPage();
      }

      app.UseIdentity();

      var TwitterKey = Configuration[$"{Configuration["TwitterKeyConfigName"]}"];
      var TwitterSecret = Configuration[$"{Configuration["TwitterSecretConfigName"]}"];

      if (TwitterKey != null && TwitterSecret != null)
      {
        app.UseTwitterAuthentication(new TwitterOptions()
        {
          ConsumerKey = TwitterKey,
          ConsumerSecret = TwitterSecret
        });
      }

      var FacebookAppId = Configuration[$"{Configuration["FacebookAppIdConfigName"]}"];
      var FacebookAppSecret = Configuration[$"{Configuration["FacebookAppSecretConfigName"]}"];

      if (FacebookAppId != null && FacebookAppSecret != null)
      {
        app.UseFacebookAuthentication(new FacebookOptions()
        {
          AppId = FacebookAppId,
          AppSecret = FacebookAppSecret
        });
      }

      app.UseMvcWithDefaultRoute();

    }

    private void InitializeContainer(IApplicationBuilder app)
    {


      _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();



      // Add application presentation components:
      _container.RegisterMvcControllers(app);
      _container.RegisterMvcViewComponents(app);

      // Cross-wire ASP.NET services (if any). For instance:
      _container.RegisterSingleton(app.ApplicationServices.GetService<ILoggerFactory>());


      _container.Register<BlogDbContext>(GetAspNetServiceProvider<BlogDbContext>(app), Lifestyle.Scoped);
      _container.Register<UserManager<ApplicationUser>>(GetAspNetServiceProvider<UserManager<ApplicationUser>>(app), Lifestyle.Scoped);
      _container.Register<RoleManager<IdentityRole>>(GetAspNetServiceProvider<RoleManager<IdentityRole>>(app), Lifestyle.Scoped);
      _container.Register<SignInManager<ApplicationUser>>(GetAspNetServiceProvider<SignInManager<ApplicationUser>>(app), Lifestyle.Scoped);
      _container.Register<IPasswordHasher<ApplicationUser>>(GetAspNetServiceProvider<IPasswordHasher<ApplicationUser>>(app), Lifestyle.Scoped);
      _container.Register<IPasswordValidator<ApplicationUser>>(GetAspNetServiceProvider<IPasswordValidator<ApplicationUser>>(app), Lifestyle.Scoped);
      _container.Register<IUserValidator<ApplicationUser>>(GetAspNetServiceProvider<IUserValidator<ApplicationUser>>(app), Lifestyle.Scoped);

      _container.AddEFQueries();
      _container.AddEFCommands();

      // NOTE: Do prevent cross-wired instances as much as possible.
      // See: https://simpleinjector.org/blog/2016/07/
    }

    // From Simple Injector Issue ... forgot which one : (
    private Func<T> GetAspNetServiceProvider<T>(IApplicationBuilder app)
    {
      var appServices = app.ApplicationServices;
      var accessor = appServices.GetRequiredService<IHttpContextAccessor>();
      return () => {
        var services = accessor.HttpContext != null
            ? accessor.HttpContext.RequestServices
            : this._container.IsVerifying ? appServices : null;
        if (services == null) throw new InvalidOperationException("No HttpContext");
        return services.GetRequiredService<T>();
      };
    }
  }
}

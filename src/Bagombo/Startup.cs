using Bagombo.AuthHandlers;
using Bagombo.Controllers;
using Bagombo.Data.Command.EFCoreCommandHandlers;
using Bagombo.Data.Query.EFCoreQueryHandlers;
using Bagombo.EFCore;
using Bagombo.Models;
using Bagombo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WilderMinds.MetaWeblog;

namespace Bagombo
{
  public class Startup
  {
    private readonly IConfiguration _configuration;
    private readonly Container _container = new Container();

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

      _configuration = builder.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
      
      // This is for a mutli-tenant Environment, so the ConnectionString Env Var name can be set
      // In AppSettings.json
      var connectionStringConfigName = _configuration["ConnectionStringConfigName"];
      var connectionString = _configuration[$"{connectionStringConfigName}"];

      if (connectionString == null)
      {
        connectionString = _configuration["ConnectionString"];
        if (connectionString == null)
        {
          throw new Exception("Unable to determine the Connection String to the database.");
        }
      }

      services.Configure<BagomboSettings>(_configuration.GetSection("BagomboSettings"));

      services.AddSession();

      services.AddMvc().AddJsonOptions(options =>
      {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
      });

      services.AddDbContext<BlogDbContext>(options => {
        options.UseSqlServer(connectionString);
      });

      services.AddDbContext<ApplicationDbContext>(options => {
        options.UseSqlServer(connectionString);
      });

      services.AddIdentity<ApplicationUser, IdentityRole>(opts => {
        opts.User.RequireUniqueEmail = true;
      }).AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

      var authBuilder = services.AddAuthentication();

      var twitterKey = _configuration[$"{_configuration["TwitterKeyConfigName"]}"];
      var twitterSecret = _configuration[$"{_configuration["TwitterSecretConfigName"]}"];

      if (twitterKey != null && twitterSecret != null)
      {
        authBuilder.AddTwitter(opts =>
        {
          opts.ConsumerKey = twitterKey;
          opts.ConsumerSecret = twitterSecret;
        });
      }

      var facebookAppId = _configuration[$"{_configuration["FacebookAppIdConfigName"]}"];
      var facebookAppSecret = _configuration[$"{_configuration["FacebookAppSecretConfigName"]}"];

      if (facebookAppId != null && facebookAppSecret != null)
      {
        authBuilder.AddFacebook(opts =>
        {
          opts.AppId = facebookAppId;
          opts.AppSecret = facebookAppSecret;
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
        opts.AddPolicy("EditAuthorProfile", policy =>
        {
          policy.Requirements.Add(new AuthorIsUserRequirement());
        });
      });

      services.AddScoped<IAuthorizationHandler>(p => new SimpleInjectorAuthorizationHandler(_container));

      IntegrateSimpleInjector(services);
    }

    public void IntegrateSimpleInjector(IServiceCollection services)
    {
      _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();


      _container.RegisterCollection<IAuthorizationHandler>(new[] { typeof(EditBlogPostAuthorizationHandler), typeof(EditAuthorProfileAuthorizationHandler) });

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

      loggerFactory.AddConsole(_configuration.GetSection("Logging"));
      loggerFactory.AddFile(_configuration.GetSection("Logging"));

      app.UseStaticFiles();

      var settings = _container.GetInstance<IOptions<BagomboSettings>>();

      if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), settings.Value.PostImagesRelativePath)))
      {
        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), settings.Value.PostImagesRelativePath));
      }

      app.UseStaticFiles(new StaticFileOptions()
      {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), settings.Value.PostImagesRelativePath)),
        RequestPath = new PathString($"/{settings.Value.PostImagesRelativePath}")
      });

      app.UseSession();

      if (env.IsDevelopment())
      {
        app.UseStatusCodePages();
        app.UseDeveloperExceptionPage();
      }

      // need this for metaweblog to work with SimpleInjector : (
      app.Use(async (context, next) =>
      {
        if (context.Request.Method == "POST" &&
        context.Request.Path.StartsWithSegments("/metaweblog") &&
        context.Request != null &&
        context.Request.ContentType.ToLower().Contains("text/xml"))
        {
          context.Response.ContentType = "text/xml";
          var rdr = new StreamReader(context.Request.Body);
          var xml = rdr.ReadToEnd();
          //_logger.LogInformation($"Request XMLRPC: {xml}");
          var mwlp = _container.GetInstance<MetaWeblogService>();
          var result = mwlp.Invoke(xml);
          //_logger.LogInformation($"Result XMLRPC: {result}");
          await context.Response.WriteAsync(result, Encoding.UTF8);
          return;
        }

        // Continue On
        await next.Invoke();

      });

      app.UseAuthentication();

      app.UseMvcWithDefaultRoute();

    }

    private void InitializeContainer(IApplicationBuilder app)
    {

      // Add application presentation components:
      _container.RegisterMvcControllers(app);
      _container.RegisterMvcViewComponents(app);

      _container.Register<IImageService, FileSystemImageService>();
      _container.Register<MetaWeblogService>();
      _container.Register<IMetaWeblogProvider, MetaWebLogProvider>();
      // Cross-wire ASP.NET services (if any). For instance:
      _container.RegisterSingleton(app.ApplicationServices.GetService<ILoggerFactory>());

      _container.CrossWire<IHttpContextAccessor>(app);
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
      _container.CrossWire<ILogger<MetaWeblogService>>(app);
      _container.CrossWire<ILogger<MetaWebLogProvider>>(app);
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

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using blog.Models;
using blog.EFCore;
using blog.data.Query.EFCoreQueryHandlers;
using Microsoft.Extensions.Configuration.UserSecrets;


namespace blog
{
  public class Startup
  {
    IConfiguration Configuration;
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

      services.AddEFQueries();

      services.AddIdentity<ApplicationUser, IdentityRole>(opts => {
        opts.User.RequireUniqueEmail = true;
      }).AddEntityFrameworkStores<BlogDbContext>()
        .AddDefaultTokenProviders();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole();
      loggerFactory.AddFile("Logs/ts-{Date}.txt");

      app.UseStaticFiles();

      if (env.IsDevelopment())
      {
        app.UseStatusCodePages();
        app.UseDeveloperExceptionPage();
        app.UseBrowserLink();
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

      BlogDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();
      BlogDbContext.CreateAuthorRole(app.ApplicationServices).Wait();
    }
  }
}

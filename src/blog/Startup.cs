using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using blog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Twitter;
using blog.Models;

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
          .AddEnvironmentVariables()
          .AddUserSecrets();

      Configuration = builder.Build();
    }
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      var ConnectionString = Configuration["ConnectionString"];

      services.AddDbContext<BlogContext>(options => options.UseSqlServer(ConnectionString));

      services.AddMvc().AddJsonOptions(options =>
      {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
      });

      services.AddDbContext<BlogIdentityDbContext>(options =>
                  options.UseSqlServer(Configuration["ConnectionString"]));

      services.AddIdentity<ApplicationUser, IdentityRole>(opts => {
        opts.User.RequireUniqueEmail = true;
      }).AddEntityFrameworkStores<BlogIdentityDbContext>()
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

      app.UseTwitterAuthentication(new TwitterOptions()
      {
        ConsumerKey = Configuration["TwitterKey"],
        ConsumerSecret = Configuration["TwitterSecret"]
      });

      app.UseMvcWithDefaultRoute();

      BlogIdentityDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();
    }
  }
}

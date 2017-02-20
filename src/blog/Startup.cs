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

      Configuration = builder.Build();
    }
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      var ConnectionString = Configuration["Database:ConnectionString"];

      services.AddDbContext<BlogContext>(options => options.UseSqlServer(ConnectionString));
      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole();

      app.UseStaticFiles();

      if (env.IsDevelopment())
      {
        app.UseStatusCodePages();
        app.UseDeveloperExceptionPage();
      }

      app.UseMvcWithDefaultRoute();
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

using blog.Models;

namespace blog.Data
{
  public class BlogIdentityDbContext : IdentityDbContext<ApplicationUser>
  {
    public BlogIdentityDbContext(DbContextOptions<BlogIdentityDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      // Customize the ASP.NET Identity model and override the defaults if needed.
      // For example, you can rename the ASP.NET Identity table names and more.
      // Add your customizations after calling base.OnModelCreating(builder);
    }

    public static async Task CreateAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
    {
      UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
      RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

      string userName = configuration["Data:AdminUser:Name"];
      string email = configuration["Data:AdminUser:Email"];
      string password = configuration["Data:Adminuser:Password"];
      string role = configuration["Data:AdminUser:Role"];

      if (await userManager.FindByNameAsync(userName) == null)
      {
        if (await roleManager.FindByNameAsync(role) == null)
        {
          await roleManager.CreateAsync(new IdentityRole(role));
        }
        ApplicationUser user = new ApplicationUser
        {
          UserName = userName,
          Email = email
        };
        IdentityResult result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
          await userManager.AddToRoleAsync(user, role);
        }
      }
    }
  }

}

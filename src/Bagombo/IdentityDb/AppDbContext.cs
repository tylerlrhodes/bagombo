using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Bagombo.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Bagombo
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      // Customize the ASP.NET Identity model and override the defaults if needed.
      // For example, you can rename the ASP.NET Identity table names and more.
      // Add your customizations after calling base.OnModelCreating(builder);
      builder.Entity<ApplicationUser>()
        .HasMany(e => e.Claims)
        .WithOne()
        .HasForeignKey(e => e.UserId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<ApplicationUser>()
          .HasMany(e => e.Logins)
          .WithOne()
          .HasForeignKey(e => e.UserId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<ApplicationUser>()
          .HasMany(e => e.Roles)
          .WithOne()
          .HasForeignKey(e => e.UserId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);
    }

    public static async Task CreateAuthorRole(IServiceProvider container)
    {
      using (var serviceScope = container.GetRequiredService<IServiceScopeFactory>().CreateScope())
      {
        RoleManager<IdentityRole> roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

        if (await roleManager.FindByNameAsync("Authors") == null)
        {
          IdentityResult result = await roleManager.CreateAsync(new IdentityRole("Authors"));
          if (!result.Succeeded)
          {
            throw new Exception("Error creating authors role!");
          }
        }
      }
    }
    public static async Task CreateAdminAccount(IServiceProvider container, IConfiguration configuration)
    {
      using (var serviceScope = container.GetRequiredService<IServiceScopeFactory>().CreateScope())
      {
        UserManager<ApplicationUser> userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
        RoleManager<IdentityRole> roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

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
}

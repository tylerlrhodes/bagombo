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
  public class BlogDbContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Feature> Features { get; set; }
    public DbSet<BlogPostFeature> BlogPostFeature { get; set; }
    public DbSet<BlogPostCategory> BlogPostCategory { get; set; }
    public DbSet<Category> Categories { get; set; }

    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      // Customize the ASP.NET Identity model and override the defaults if needed.
      // For example, you can rename the ASP.NET Identity table names and more.
      // Add your customizations after calling base.OnModelCreating(builder);
      builder.Entity<Author>().ToTable("Author");
      builder.Entity<Author>().HasAlternateKey(e => new { e.FirstName, e.LastName });
      builder.Entity<Author>().HasOne(e => e.ApplicationUser)
                              .WithOne(au => au.Author)
                              .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.SetNull);
      //.OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.);

      builder.Entity<ApplicationUser>().HasOne(e => e.Author)
                                       .WithOne(a => a.ApplicationUser)
                                       .HasForeignKey<Author>(a => a.ApplicationUserId);


      builder.Entity<Author>().HasIndex(a => a.ApplicationUserId)
                              .IsUnique(false);

      builder.Entity<Author>().HasMany(a => a.BlogPosts)
                              .WithOne(bp => bp.Author)
                              .IsRequired(false)
                              .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.SetNull);


      builder.Entity<BlogPost>().ToTable("BlogPost");

      builder.Entity<BlogPost>().HasOne(bp => bp.Author)
                                .WithMany(a => a.BlogPosts)
                                .HasForeignKey("AuthorId")
                                .IsRequired(false);
                                
      builder.Entity<Feature>().ToTable("Feature");
      builder.Entity<Category>().ToTable("Category");

      builder.Entity<BlogPostFeature>().HasKey(bpf => new { bpf.FeatureId, bpf.BlogPostId });
      builder.Entity<BlogPostFeature>().HasOne(bpf => bpf.BlogPost)
                                       .WithMany(bp => bp.Features)
                                       .HasForeignKey(bpf => bpf.BlogPostId);
      builder.Entity<BlogPostFeature>().HasOne(bpf => bpf.Feature)
                                       .WithMany(f => f.BlogPosts)
                                       .HasForeignKey(bpf => bpf.FeatureId);

      builder.Entity<BlogPostCategory>().HasKey(bpc => new { bpc.BlogPostId, bpc.CategoryId });
      builder.Entity<BlogPostCategory>().HasOne(bpc => bpc.BlogPost)
                                        .WithMany(bp => bp.Categories)
                                        .HasForeignKey(bpc => bpc.BlogPostId);
      builder.Entity<BlogPostCategory>().HasOne(bpc => bpc.Category)
                                        .WithMany(c => c.BlogPosts)
                                        .HasForeignKey(bpc => bpc.CategoryId);
    }

    public static async Task CreateAuthorRole(IServiceProvider serviceProvider)
    {
      RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

      if (await roleManager.FindByNameAsync("Authors") == null)
      {
        IdentityResult result = await roleManager.CreateAsync(new IdentityRole("Authors"));
        if (!result.Succeeded)
        {
          throw new Exception("Error creating authors role!");
        }
      }
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

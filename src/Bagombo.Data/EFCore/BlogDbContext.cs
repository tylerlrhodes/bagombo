using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using SimpleInjector;
using SimpleInjector.Lifestyles;

using Bagombo.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Bagombo.EFCore
{
  public class BlogDbContext : DbContext
  {
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<BlogPostTopic> BlogPostTopic { get; set; }
    public DbSet<BlogPostCategory> BlogPostCategory { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.QueryClientEvaluationWarning));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<Author>().ToTable("Author");
      builder.Entity<Author>().HasMany(a => a.BlogPosts)
        .WithOne(bp => bp.Author)
        .IsRequired(false)
        .OnDelete(DeleteBehavior.SetNull);

      builder.Entity<BlogPost>().ToTable("BlogPost");
      builder.Entity<BlogPost>().HasOne(bp => bp.Author)
        .WithMany(a => a.BlogPosts)
        .HasForeignKey("AuthorId")
        .IsRequired(false);

      builder.Entity<BlogPost>().HasIndex(bp => bp.Slug)
        .IsUnique();

      //builder.Entity<Comment>().ToTable("Comment");
      //builder.Entity<Comment>().HasOne(c => c.BlogPost)
      //  .WithMany(c => c.Comments);

      //builder.Entity<BlogPost>().HasMany(bp => bp.Comments)
      //  .WithOne(c => c.BlogPost)
      //  .IsRequired(false);

      builder.Entity<Topic>().ToTable("Topic");
      builder.Entity<Topic>().HasIndex(t => t.Title)
        .IsUnique();

      builder.Entity<Category>().ToTable("Category");
      builder.Entity<Category>().HasIndex(c => c.Name)
        .IsUnique();

      builder.Entity<BlogPostTopic>().HasKey(bpf => new { bpf.TopicId, bpf.BlogPostId });
      builder.Entity<BlogPostTopic>().HasOne(bpf => bpf.BlogPost)
        .WithMany(bp => bp.BlogPostTopic)
        .HasForeignKey(bpf => bpf.BlogPostId);
      builder.Entity<BlogPostTopic>().HasOne(bpf => bpf.Topic)
        .WithMany(f => f.BlogPosts)
        .HasForeignKey(bpf => bpf.TopicId);

      builder.Entity<BlogPostCategory>().HasKey(bpc => new { bpc.BlogPostId, bpc.CategoryId });
      builder.Entity<BlogPostCategory>().HasOne(bpc => bpc.BlogPost)
        .WithMany(bp => bp.BlogPostCategory)
        .HasForeignKey(bpc => bpc.BlogPostId);
      builder.Entity<BlogPostCategory>().HasOne(bpc => bpc.Category)
        .WithMany(c => c.BlogPosts)
        .HasForeignKey(bpc => bpc.CategoryId);
    }

    public static async Task UpdateSlugs(IServiceProvider container)
    {
      using (var serviceScope = container.GetRequiredService<IServiceScopeFactory>().CreateScope())
      {
        var _context = serviceScope.ServiceProvider.GetRequiredService<BlogDbContext>();

        foreach (var bp in _context.BlogPosts)
        {
          bp.Slug = BlogPostExtensions.CreateSlug(bp.Title);
        }

        await _context.SaveChangesAsync();
      }
    }
  }

}

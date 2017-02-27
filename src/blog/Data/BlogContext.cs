using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using blog.Models;

namespace blog.Data
{
  public class BlogContext : DbContext
  {
    public BlogContext(DbContextOptions<BlogContext> options) : base(options)
    {

    }
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Feature> Features { get; set; }
    public DbSet<BlogPostFeature> BlogPostFeature { get; set; }
    public DbSet<BlogPostCategory> BlogPostCategory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<BlogPost>().ToTable("BlogPost");
      modelBuilder.Entity<Author>().ToTable("Author");
      modelBuilder.Entity<Feature>().ToTable("Feature");

      modelBuilder.Entity<Author>().HasAlternateKey(e => new { e.FirstName, e.LastName });

      modelBuilder.Entity<BlogPostFeature>().HasKey(bpf => new { bpf.FeatureId, bpf.BlogPostId });
      modelBuilder.Entity<BlogPostFeature>().HasOne(bpf => bpf.BlogPost).WithMany(bp => bp.Features).HasForeignKey(bpf => bpf.BlogPostId);
      modelBuilder.Entity<BlogPostFeature>().HasOne(bpf => bpf.Feature).WithMany(f => f.BlogPosts).HasForeignKey(bpf => bpf.FeatureId);

      modelBuilder.Entity<BlogPostCategory>().HasKey(bpc => new { bpc.BlogPostId, bpc.CategoryId });
      modelBuilder.Entity<BlogPostCategory>().HasOne(bpc => bpc.BlogPost).WithMany(bp => bp.Categories).HasForeignKey(bpc => bpc.BlogPostId);
      modelBuilder.Entity<BlogPostCategory>().HasOne(bpc => bpc.Category).WithMany(c => c.BlogPosts).HasForeignKey(bpc => bpc.CategoryId);

    }
  }
}

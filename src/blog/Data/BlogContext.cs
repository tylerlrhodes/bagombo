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
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<BlogPost>().ToTable("BlogPost");
      modelBuilder.Entity<Author>().ToTable("Author");
    }
  }
}

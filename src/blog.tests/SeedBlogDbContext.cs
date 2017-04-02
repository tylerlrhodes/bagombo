using System;
using System.Collections.Generic;
using System.Text;
using blog.EFCore;
using blog.Models;

namespace blog.tests
{
  public class SeedBlogDbContext
  {
    public static void SeedData(BlogDbContext _context)
    {
      // Seed the given context with data for unit testing

      Author JohnSmith = new Author()
      {
        ApplicationUserId = Guid.NewGuid().ToString(),
        FirstName = "John",
        LastName = "Smith",
        Biography = "Born someday",
      };

      Author JaneSmith = new Author()
      {
        ApplicationUserId = Guid.NewGuid().ToString(),
        FirstName = "Jane",
        LastName = "Smith",
        Biography = "Born some other day"
      };

      _context.Authors.Add(JohnSmith);
      _context.Authors.Add(JaneSmith);

      BlogPost bp1 = new BlogPost()
      {
        Author = JohnSmith,
        Title = "bp1",
        Description = "bp1 Description",
        Content = "bp 1 Content xyz123",
        CreatedAt = new DateTime(2017, 3, 30, 14, 20, 20),
        Public = true,
        ModifiedAt = DateTime.Now,
        PublishOn = new DateTime(2017, 4, 1, 0, 0, 0)
      };

      BlogPost bp2 = new BlogPost()
      {
        Author = JaneSmith,
        Title = "bp2",
        Description = "bp2 Description",
        Content = "bp 2 Content xyz123  zyx321",
        CreatedAt = new DateTime(2017, 3, 29, 14, 20, 20),
        Public = true,
        ModifiedAt = DateTime.Now,
        PublishOn = new DateTime(2017, 3, 30, 0, 0, 0)
      };

      BlogPost bp3 = new BlogPost()
      {
        Author = JaneSmith,
        Title = "bp3",
        Description = "bp3 Description",
        Content = "bp 3 Content xyz123  zyx321 abcdef",
        CreatedAt = new DateTime(2017, 3, 29, 14, 20, 20),
        Public = true,
        ModifiedAt = DateTime.Now,
        PublishOn = new DateTime(2017, 3, 30, 0, 0, 0)
      };

      _context.BlogPosts.Add(bp1);
      _context.BlogPosts.Add(bp2);
      _context.BlogPosts.Add(bp3);

      Feature f1 = new Feature()
      {
        Title = "Feature 1",
        Description = "Feature 1 Description"
      };

      Feature f2 = new Feature()
      {
        Title = "Feature 2",
        Description = "Feature 2 Description"
      };

      _context.Features.Add(f1);
      _context.Features.Add(f2);

      var bpfList = new List<BlogPostFeature>()
      {
        new BlogPostFeature()
        {
          BlogPost = bp1,
          Feature = f1
        },
        new BlogPostFeature()
        {
          BlogPost = bp1,
          Feature = f2
        },
        new BlogPostFeature()
        {
          BlogPost = bp2,
          Feature = f1
        },
        new BlogPostFeature()
        {
          BlogPost = bp3,
          Feature = f1
        }
      };

      _context.BlogPostFeature.AddRange(bpfList);

      Category c1 = new Category()
      {
        Name = "c1",
        Description = "c1 Description"
      };

      Category c2 = new Category()
      {
        Name = "c2",
        Description = "c2 Description"
      };

      Category c3 = new Category()
      {
        Name = "c3",
        Description = "c3 Description"
      };

      _context.Categories.Add(c1);
      _context.Categories.Add(c2);
      _context.Categories.Add(c3);

      var bpcList = new List<BlogPostCategory>()
      {
        new BlogPostCategory()
        {
          BlogPost = bp1,
          Category = c1
        },
        new BlogPostCategory()
        {
          BlogPost = bp1,
          Category = c2
        },
        new BlogPostCategory()
        {
          BlogPost = bp1,
          Category = c3
        },
        new BlogPostCategory()
        {
          BlogPost = bp2,
          Category = c2
        },
        new BlogPostCategory()
        {
          BlogPost = bp2,
          Category = c3
        },
        new BlogPostCategory()
        {
          BlogPost = bp3,
          Category = c3
        }
      };

      _context.BlogPostCategory.AddRange(bpcList);

      _context.SaveChanges();

    }
  }
}

using System;
using Xunit;
using Bagombo.EFCore;
using Bagombo.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace Bagombo.tests
{
  public class TestInMemoryFeatureParity
  {
    /// <summary>
    /// Test basic features of the EF Core In Memory database to see if its suitable for Unit Testing
    /// </summary>
    [Fact]
    public void TestInMemoryDbContext()
    {
      var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

      BlogDbContext db = new BlogDbContext(optionsBuilder);

      Author authorTest = new Author()
      {
        ApplicationUserId = Guid.NewGuid().ToString(),
        FirstName = "John",
        LastName = "Smith",
        Biography = "Born somewhere, someday."
      };

      db.Authors.Add(authorTest);

      db.SaveChanges();

      var x = (from author in db.Authors
               select author).FirstOrDefault();

      Assert.NotNull(x);
      Assert.Equal(x.FirstName, authorTest.FirstName);
      Assert.Equal(x.LastName, authorTest.LastName);
      Assert.Equal(x.Biography, authorTest.Biography);

      BlogPost bp = new BlogPost()
      {
        Author = authorTest,
        Title = "Hello",
        Content = "Content",
        Description = "Description",
        CreatedAt = DateTime.Now,
        Public = true,
        PublishOn = DateTime.Now,
        ModifiedAt = DateTime.Now,
      };

      db.BlogPosts.Add(bp);

      db.SaveChanges();

      var y = db.BlogPosts.Include(b => b.Author).Select(b => b).FirstOrDefault();

      Assert.Equal(y.Author.FirstName, authorTest.FirstName);

      var catList = new List<Category>()
      {
        new Category()
        {
          Name = "Cat1"
        },
        new Category()
        {
          Name = "Cat2"
        }
      };

      db.Categories.AddRange(catList);

      var bpcList = new List<BlogPostCategory>()
      {
        new BlogPostCategory()
        {
          BlogPost = bp,
          Category = catList[0]
        },
        new BlogPostCategory()
        {
          BlogPost = bp,
          Category = catList[1]
        }
      };

      db.BlogPostCategory.AddRange(bpcList);

      db.SaveChanges();

      var featureList = new List<Topic>()
      {
        new Topic()
        {
          Title = "Feature 1"
        },
        new Topic()
        {
          Title = "Feature 2"
        }
      };

      db.Topic.AddRange(featureList);

      db.SaveChanges();

      var bpfList = new List<BlogPostTopic>()
      {
        new BlogPostTopic()
        {
          BlogPost = bp,
          Topic = featureList[0]
        },
        new BlogPostTopic()
        {
          BlogPost = bp,
          Topic =featureList[1]
        }
      };

      db.BlogPostTopic.AddRange(bpfList);

      db.SaveChanges();

      var bpTest = db.BlogPosts
        .Include(b => b.Author)
        .Include(b => b.BlogPostTopic)
          .ThenInclude(bbpf => bbpf.Topic)
        .Include(b => b.BlogPostCategory)
          .ThenInclude(bbpc => bbpc.Category)
        .Select(b => b)
        .FirstOrDefault();

      Assert.Equal(bpTest.BlogPostTopic.Select(bpf => bpf.Topic).Count(), 2);
      Assert.Equal(bpTest.BlogPostCategory.Select(bpc => bpc.Category).Count(), 2);
      Assert.Equal(bpTest.Author.FirstName, authorTest.FirstName);

    }
  }
}

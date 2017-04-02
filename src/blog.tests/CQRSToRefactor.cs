using System;
using System.Collections.Generic;
using System.Text;
using blog.EFCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Microsoft.EntityFrameworkCore;
using blog.data.Query;
using blog.Models;

namespace blog.tests
{
  public class CQRSToRefactor
  {
    [Fact]
    public async void GetRecentBlogPostsTest()
    {
      ServiceCollection services = new ServiceCollection();

      services.AddScoped<BlogDbContext>(o => { return new BlogDbContext(new DbContextOptionsBuilder<BlogDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options); });

      services.AddTransient<IQueryHandlerAsync<GetRecentBlogPosts, IList<BlogPost>>, GetRecentBlogPostsEFQueryHandler>();

      var sp = services.BuildServiceProvider();

      var db = sp.GetService<BlogDbContext>();

      SeedBlogDbContext.SeedData(db);

      QueryProcessorAsync qpa = new QueryProcessorAsync(sp);

      GetRecentBlogPosts grbp = new GetRecentBlogPosts()
      {
        NumberOfPostsToGet = 2
      };

      var x = await qpa.ProcessAsync(grbp);

      Assert.Equal(x.Count, 2);

      grbp.NumberOfPostsToGet = 3;

      x = await qpa.ProcessAsync(grbp);

      Assert.Equal(x.Count, 3);

    }
  }
}

using System;
using Xunit;
using blog.Data;
using blog.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace blog.tests
{
  public class UnitTest1
  {
    [Fact]
    public void Test1()
    {
      Assert.Equal(0, 0);
    }

    [Fact]
    public void TestDbContext()
    {
      var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

      BlogDbContext db = new BlogDbContext(optionsBuilder);
      

      
    }
  }
}

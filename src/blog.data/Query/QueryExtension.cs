using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using blog.Models;

namespace blog.data.Query
{
  public static class QueryExtension
  {
    public static void AddQueries(this IServiceCollection services)
    {
      services.AddTransient<IQueryHandlerAsync<GetRecentBlogPosts, IList<BlogPost>>, GetRecentBlogPostsEFQueryHandler>();
      services.AddTransient<QueryProcessorAsync, QueryProcessorAsync>();
    }
  }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using blog.Models;
using blog.Models.ViewModels.Home;

namespace blog.data.Query.EFCoreQueryHandlers
{
  public static class EFQueryExtensions
  {
    public static void AddEFQueries(this IServiceCollection services)
    {
      services.AddTransient<IQueryHandlerAsync<GetRecentBlogPosts, IList<BlogPost>>, GetRecentBlogPostsEFQueryHandler>();
      services.AddTransient<IQueryHandlerAsync<GetBlogPostsBySearchText, IList<ViewSearchResultBlogPostViewModel>>, GetBlogPostsBySearchTextEFQueryHandler>();
      services.AddTransient<QueryProcessorAsync, QueryProcessorAsync>();
    }
  }
}

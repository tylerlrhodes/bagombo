using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using blog.Models;
using blog.Models.ViewModels.Home;
using blog.data.Query.Queries;
using blog.data.Query;
using SimpleInjector;

namespace blog.data.Query.EFCoreQueryHandlers
{
  public static class EFQueryExtensions
  {
    public static void AddEFQueries(this Container container)
    {
      container.Register<IQueryHandlerAsync<GetRecentBlogPosts, IList<BlogPost>>, GetRecentBlogPostsEFQueryHandler>(Lifestyle.Scoped);
      container.Register<IQueryHandlerAsync<GetViewSearchResultBlogPostsBySearchText, IList<ViewSearchResultBlogPostViewModel>>, GetViewSearchResultBlogPostsBySearchTextEFQueryHandler>();
      container.Register<IQueryHandlerAsync<GetViewCategoryPostsByCategory, ViewCategoryPostsViewModel>, GetViewCategoryPostsByCategoryEFQueryHandler>();
      container.Register<QueryProcessorAsync, QueryProcessorAsync>(Lifestyle.Scoped);
    }
  }
}

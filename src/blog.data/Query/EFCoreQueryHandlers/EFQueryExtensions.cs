using blog.data.Query.Queries;
using blog.Models;
using blog.Models.ViewModels.Home;
using SimpleInjector;
using System.Collections.Generic;

namespace blog.data.Query.EFCoreQueryHandlers
{
  public static class EFQueryExtensions
  {
    public static void AddEFQueries(this Container container)
    {
      container.Register<IQueryHandlerAsync<GetRecentBlogPosts, IList<BlogPost>>, GetRecentBlogPostsEFQueryHandler>();
      container.Register<IQueryHandlerAsync<GetViewSearchResultBlogPostsBySearchText, IList<ViewSearchResultBlogPostViewModel>>, GetViewSearchResultBlogPostsBySearchTextEFQueryHandler>();
      container.Register<IQueryHandlerAsync<GetViewCategoryPostsByCategory, ViewCategoryPostsViewModel>, GetViewCategoryPostsByCategoryEFQueryHandler>();
      container.Register<IQueryHandlerAsync<GetViewAllPostsByCategory, ViewAllPostsViewModel>, GetViewAllPostsByCategoryEFQueryHandler>();
      container.Register<IQueryHandlerAsync<GetViewAllPostsByDate, ViewAllPostsViewModel>, GetViewAllPostsByDateEFQueryHandler>();
      container.Register<IQueryHandlerAsync<GetViewFeaturePostsByFeature, ViewFeaturePostsViewModel>, GetViewFeaturePostsByFeatureEFQueryHandler>();
      container.Register<IQueryHandlerAsync<GetViewFeatures, ViewFeaturesViewModel>, GetViewFeaturesEFQueryHandler>();
      container.Register<IQueryHandlerAsync<GetViewBlogPostById, ViewBlogPostViewModel>, GetViewBlogPostByIdEFQueryHandler>();

      container.Register<QueryProcessorAsync, QueryProcessorAsync>(Lifestyle.Scoped);
    }
  }
}

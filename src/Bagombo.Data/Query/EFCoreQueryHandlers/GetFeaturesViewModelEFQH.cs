using Bagombo.Data.Query.Queries;
using Bagombo.EFCore;
using Bagombo.Models.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetFeaturesViewModelEFQH : IQueryHandlerAsync<GetFeaturesViewModelQuery, FeaturesViewModel>
  {
    BlogDbContext _context;

    public GetFeaturesViewModelEFQH(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<FeaturesViewModel> HandleAsync(GetFeaturesViewModelQuery query)
    {
      // bug in EF Core that needs a bit more code here than otherwise
      // see: https://github.com/aspnet/EntityFramework/issues/7714

      FeaturesViewModel vfvm = new FeaturesViewModel();

      // cant select new into a defined type so have to use anon type for the select new here due to EF Core bug
      // code after is a work-around

      var x = from feature in _context.Features.AsNoTracking()
              select new
              {
                Title = feature.Title,
                Description = feature.Description,
                Id = feature.Id,
                BlogCount = (from posts in _context.BlogPostFeature
                             where posts.FeatureId == feature.Id && posts.BlogPost.Public == true && posts.BlogPost.PublishOn < DateTime.Now
                             select posts).AsNoTracking().Count()
              };

      List<FeatureWithBlogCountViewModel> featureList = new List<FeatureWithBlogCountViewModel>();

      foreach (var feature in await x.OrderByDescending(f => f.BlogCount).ToListAsync())
      {
        FeatureWithBlogCountViewModel fvm = new FeatureWithBlogCountViewModel()
        {
          BlogCount = feature.BlogCount,
          Title = feature.Title,
          Description = feature.Description,
          Id = feature.Id
        };
        featureList.Add(fvm);
      }

      vfvm.Features = featureList;

      return vfvm;

    }
  }
}

using blog.data.Query.Queries;
using blog.EFCore;
using blog.Models.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace blog.data.Query.EFCoreQueryHandlers
{
  public class GetViewFeaturesEFQueryHandler : IQueryHandlerAsync<GetViewFeatures, ViewFeaturesViewModel>
  {
    BlogDbContext _context;

    public GetViewFeaturesEFQueryHandler(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<ViewFeaturesViewModel> HandleAsync(GetViewFeatures query)
    {
      // bug in EF Core that needs a bit more code here than otherwise
      // see: https://github.com/aspnet/EntityFramework/issues/7714

      ViewFeaturesViewModel vfvm = new ViewFeaturesViewModel();

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

      List<FeatureViewModel> featureList = new List<FeatureViewModel>();

      foreach (var feature in await x.OrderByDescending(f => f.BlogCount).ToListAsync())
      {
        FeatureViewModel fvm = new FeatureViewModel()
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

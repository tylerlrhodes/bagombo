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
  public class GetTopicsViewModelEFQH : EFQHBase, IQueryHandlerAsync<GetTopicsViewModelQuery, TopicsViewModel>
  {
    public GetTopicsViewModelEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<TopicsViewModel> HandleAsync(GetTopicsViewModelQuery query)
    {
      // bug in EF Core that needs a bit more code here than otherwise
      // see: https://github.com/aspnet/EntityFramework/issues/7714

      TopicsViewModel vfvm = new TopicsViewModel();

      // cant select new into a defined type so have to use anon type for the select new here due to EF Core bug
      // code after is a work-around

      var x = from feature in _context.Topic.AsNoTracking()
              select new
              {
                Title = feature.Title,
                Description = feature.Description,
                Id = feature.Id,
                BlogCount = (from posts in _context.BlogPostTopic
                             where posts.TopicId == feature.Id && posts.BlogPost.Public == true && posts.BlogPost.PublishOn < DateTime.Now
                             select posts).AsNoTracking().Count()
              };

      List<TopicWithBlogCountViewModel> featureList = new List<TopicWithBlogCountViewModel>();

      foreach (var feature in await x.OrderByDescending(f => f.BlogCount).ToListAsync())
      {
        TopicWithBlogCountViewModel fvm = new TopicWithBlogCountViewModel()
        {
          BlogCount = feature.BlogCount,
          Title = feature.Title,
          Description = feature.Description,
          Id = feature.Id
        };
        featureList.Add(fvm);
      }

      vfvm.Topics = featureList;

      return vfvm;

    }
  }
}

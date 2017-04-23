using Bagombo.Data.Query.Queries;
using Bagombo.EFCore;
using Bagombo.Models.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  class GetFeaturePostsByFeatureViewModelEFQH : EFQHBase, IQueryHandlerAsync<GetFeaturePostsByFeatureViewModelQuery, FeaturePostsViewModel>
  {
    public GetFeaturePostsByFeatureViewModelEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<FeaturePostsViewModel> HandleAsync(GetFeaturePostsByFeatureViewModelQuery query)
    {

      var feature = await _context.Features.FindAsync(query.Id);

      if (feature == null)
      {
        return null;
      }

      var bpfs = await _context.BlogPostFeature
                              .AsNoTracking()
                              .Where(bpf => bpf.FeatureId == feature.Id && bpf.BlogPost.Public == true && bpf.BlogPost.PublishOn < DateTime.Now)
                              .Include(bpf => bpf.BlogPost)
                                .ThenInclude(bp => bp.Author)
                              .Include(bpf => bpf.BlogPost)
                                .ThenInclude(bp => bp.BlogPostCategory)
                                .ThenInclude(bpc => bpc.Category)
                              .ToListAsync();

      List<BlogPostViewModel> viewPosts = new List<BlogPostViewModel>();

      foreach (var bpf in bpfs)
      {
        var bpView = new BlogPostViewModel()
        {
          Author = $"{bpf.BlogPost.Author.FirstName} {bpf.BlogPost.Author.LastName}",
          Title = bpf.BlogPost.Title,
          Description = bpf.BlogPost.Description,
          Categories = bpf.BlogPost.BlogPostCategory.Select(c => c.Category).ToList(),
          ModifiedAt = bpf.BlogPost.ModifiedAt,
          Id = bpf.BlogPost.Id
        };
        viewPosts.Add(bpView);
      }

      return new FeaturePostsViewModel()
      {
        Feature = feature,
        BlogPosts = viewPosts
      };

    }

  }

}

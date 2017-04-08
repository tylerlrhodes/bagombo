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
  class GetViewFeaturePostsByFeatureEFQueryHandler : IQueryHandlerAsync<GetViewFeaturePostsByFeature, ViewFeaturePostsViewModel>
  {
    BlogDbContext _context;
    
    public GetViewFeaturePostsByFeatureEFQueryHandler(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<ViewFeaturePostsViewModel> HandleAsync(GetViewFeaturePostsByFeature query)
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

      List<ViewBlogPostViewModel> viewPosts = new List<ViewBlogPostViewModel>();

      foreach (var bpf in bpfs)
      {
        var bpView = new ViewBlogPostViewModel()
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

      return new ViewFeaturePostsViewModel()
      {
        Feature = feature,
        BlogPosts = viewPosts
      };

    }

  }

}

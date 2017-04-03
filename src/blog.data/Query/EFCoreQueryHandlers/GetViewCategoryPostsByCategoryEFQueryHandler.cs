using blog.data.Query.Queries;
using blog.EFCore;
using blog.Models;
using blog.Models.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog.data.Query.EFCoreQueryHandlers
{
  public class GetViewCategoryPostsByCategoryEFQueryHandler : IQueryHandlerAsync<GetViewCategoryPostsByCategory, ViewCategoryPostsViewModel>
  {
    private BlogDbContext _context;

    public GetViewCategoryPostsByCategoryEFQueryHandler(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<ViewCategoryPostsViewModel> HandleAsync(GetViewCategoryPostsByCategory query)
    {
      var category = await _context.Categories.FindAsync(query.Id);

      var vcpvm = new ViewCategoryPostsViewModel()
      {
        Category = category,
        Posts = new List<BlogPost>()
      };

      if (category != null)
      {
        var bpcs = await _context.BlogPostCategory
                                .AsNoTracking()
                                .Where(bp => bp.CategoryId == category.Id && bp.BlogPost.Public == true && bp.BlogPost.PublishOn < DateTime.Now)
                                .Include(bpc => bpc.BlogPost)
                                  .ThenInclude(bp => bp.Author)
                                .ToListAsync();

        vcpvm.Category = category;
        vcpvm.Posts = bpcs.Select(bp => bp.BlogPost).ToList();
      }

      return vcpvm;
    }
  }
}

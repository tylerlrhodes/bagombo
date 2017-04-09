using Bagombo.Data.Query.Queries;
using Bagombo.EFCore;
using Bagombo.Models;
using Bagombo.Models.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetCategoryPostsByCategoryViewModelEFQH : IQueryHandlerAsync<GetCategoryPostsByCategoryViewModelQuery, CategoryPostsViewModel>
  {
    private BlogDbContext _context;

    public GetCategoryPostsByCategoryViewModelEFQH(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<CategoryPostsViewModel> HandleAsync(GetCategoryPostsByCategoryViewModelQuery query)
    {
      var category = await _context.Categories.FindAsync(query.Id);

      var vcpvm = new CategoryPostsViewModel()
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

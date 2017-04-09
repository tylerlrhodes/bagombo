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
  public class GetAllPostsByCategoryViewModelEFQH : IQueryHandlerAsync<GetAllPostsByCategoryViewModel, AllPostsViewModel>
  {
    private BlogDbContext _context;

    public GetAllPostsByCategoryViewModelEFQH(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<AllPostsViewModel> HandleAsync(GetAllPostsByCategoryViewModel query)
    {
      var categories = await _context.Categories.AsNoTracking().ToListAsync();

      var viewCategories = new List<PostsByCategoryViewModel>();

      foreach (var c in categories)
      {
        var bpcs = await _context.BlogPostCategory
                                .AsNoTracking()
                                .Where(bp => bp.CategoryId == c.Id && bp.BlogPost.Public == true && bp.BlogPost.PublishOn < DateTime.Now)
                                .Include(bpc => bpc.BlogPost)
                                  .ThenInclude(bp => bp.Author)
                                .ToListAsync();

        var vpbc = new PostsByCategoryViewModel()
        {
          Category = c,
          Posts = bpcs.Select(bp => bp.BlogPost).ToList()
        };

        viewCategories.Add(vpbc);
      }

      return new AllPostsViewModel()
      {
        PostsByDate = null,
        SortBy = 1,
        Categories = viewCategories ?? new List<PostsByCategoryViewModel>()
      };
    }
  }
}

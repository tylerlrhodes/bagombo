using blog.data.Query.Queries;
using blog.EFCore;
using blog.Models.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blog.data.Query.EFCoreQueryHandlers
{
  public class GetViewAllPostsByCategoryEFQueryHandler : IQueryHandlerAsync<GetViewAllPostsByCategory, ViewAllPostsViewModel>
  {
    private BlogDbContext _context;

    public GetViewAllPostsByCategoryEFQueryHandler(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<ViewAllPostsViewModel> HandleAsync(GetViewAllPostsByCategory query)
    {
      var categories = await _context.Categories.AsNoTracking().ToListAsync();

      var viewCategories = new List<ViewPostsByCategory>();

      foreach (var c in categories)
      {
        var bpcs = await _context.BlogPostCategory
                                .AsNoTracking()
                                .Where(bp => bp.CategoryId == c.Id && bp.BlogPost.Public == true && bp.BlogPost.PublishOn < DateTime.Now)
                                .Include(bpc => bpc.BlogPost)
                                  .ThenInclude(bp => bp.Author)
                                .ToListAsync();

        var vpbc = new ViewPostsByCategory()
        {
          Category = c,
          Posts = bpcs.Select(bp => bp.BlogPost).ToList()
        };

        viewCategories.Add(vpbc);
      }

      return new ViewAllPostsViewModel()
      {
        PostsByDate = null,
        SortBy = 1,
        Categories = viewCategories ?? new List<ViewPostsByCategory>()
      };
    }
  }
}

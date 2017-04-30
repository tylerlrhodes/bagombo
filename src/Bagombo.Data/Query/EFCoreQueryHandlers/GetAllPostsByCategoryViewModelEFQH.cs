using Bagombo.Data.Query.Queries;
using Bagombo.EFCore;
using Bagombo.Models;
using Bagombo.Models.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetAllPostsByCategoryViewModelEFQH : EFQHBase, IQueryHandlerAsync<GetAllPostsByCategoryViewModelQuery, AllPostsViewModel>
  {
    public GetAllPostsByCategoryViewModelEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<AllPostsViewModel> HandleAsync(GetAllPostsByCategoryViewModelQuery query)
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

      var NoCategory = new Category()
      {
        Name = "No Category"
      };

      var ncbps = await _context.BlogPosts.AsNoTracking()
                                .Include(bpc => bpc.BlogPostCategory)
                                .Where(bpc => bpc.BlogPostCategory.Count == 0)
                                .Include(bp => bp.Author)
                                .ToListAsync();

      var ncvpbc = new PostsByCategoryViewModel()
      {
        Category = NoCategory,
        Posts = ncbps
      };

      viewCategories.Add(ncvpbc);

      return new AllPostsViewModel()
      {
        PostsByDate = null,
        SortBy = 1,
        Categories = viewCategories ?? new List<PostsByCategoryViewModel>()
      };
    }
  }
}

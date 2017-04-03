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
  public class GetViewAllPostsByDateEFQueryHandler : IQueryHandlerAsync<GetViewAllPostsByDate, ViewAllPostsViewModel>
  {
    BlogDbContext _context;

    public GetViewAllPostsByDateEFQueryHandler(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<ViewAllPostsViewModel> HandleAsync(GetViewAllPostsByDate query)
    {
      return new ViewAllPostsViewModel()
      {
        PostsByDate = await _context.BlogPosts
                                      .AsNoTracking()
                                      .Where(bp => bp.Public == true && bp.PublishOn < DateTime.Now)
                                      .Include(bp => bp.Author)
                                      .OrderByDescending(bp => bp.ModifiedAt)
                                      .ThenByDescending(bp => bp.PublishOn)
                                      .ToListAsync(),
        SortBy = 2,
        Categories = null
      };
    }
  }
}

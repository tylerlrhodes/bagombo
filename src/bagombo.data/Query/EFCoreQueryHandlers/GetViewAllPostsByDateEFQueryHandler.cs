using Bagombo.data.Query.Queries;
using Bagombo.EFCore;
using Bagombo.Models.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bagombo.data.Query.EFCoreQueryHandlers
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

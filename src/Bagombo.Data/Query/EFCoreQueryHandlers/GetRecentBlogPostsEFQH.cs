using System;
using System.Collections.Generic;
using System.Text;
using Bagombo.Models;
using System.Threading.Tasks;
using Bagombo.EFCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Bagombo.Data.Query.Queries;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetRecentBlogPostsEFQH : IQueryHandlerAsync<GetRecentBlogPostsQuery, IList<BlogPost>>
  {
    private BlogDbContext _context;

    public GetRecentBlogPostsEFQH(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<IList<BlogPost>> HandleAsync(GetRecentBlogPostsQuery query)
    {
      var recentPosts = await _context.BlogPosts
                                .AsNoTracking()
                                .Where(bp => bp.Public == true && bp.PublishOn <= DateTime.Now)
                                .OrderByDescending(bp => bp.ModifiedAt)
                                .ThenByDescending(bp => bp.PublishOn)
                                .Take(query.NumberOfPostsToGet)
                                .ToListAsync();

      return recentPosts;
    }
  }
}

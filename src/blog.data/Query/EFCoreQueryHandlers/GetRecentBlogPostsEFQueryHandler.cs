using System;
using System.Collections.Generic;
using System.Text;
using blog.Models;
using System.Threading.Tasks;
using blog.EFCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace blog.data.Query.EFCoreQueryHandlers
{
  public class GetRecentBlogPostsEFQueryHandler : IQueryHandlerAsync<GetRecentBlogPosts, IList<BlogPost>>
  {
    private BlogDbContext _context;

    public GetRecentBlogPostsEFQueryHandler(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<IList<BlogPost>> HandleAsync(GetRecentBlogPosts query)
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

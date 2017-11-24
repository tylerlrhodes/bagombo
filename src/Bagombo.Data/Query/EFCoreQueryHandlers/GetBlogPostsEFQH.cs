using Bagombo.Data.Query.Queries;
using Bagombo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bagombo.EFCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetBlogPostsEFQH : EFQHBase, IQueryHandlerAsync<GetBlogPostsQuery, IEnumerable<BlogPost>>
  {
    public GetBlogPostsEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<BlogPost>> HandleAsync(GetBlogPostsQuery query)
    {
      return await _context.BlogPosts
        .Where(bp => bp.PublishOn < DateTime.Now && bp.Public == true)
        .OrderByDescending(bp => bp.PublishOn)
        .Include(bp => bp.Author)
        .Include(bp => bp.BlogPostCategory)
          .ThenInclude(bpc => bpc.Category)
        .Take(50_000)
        .ToListAsync();
    }
  }
}

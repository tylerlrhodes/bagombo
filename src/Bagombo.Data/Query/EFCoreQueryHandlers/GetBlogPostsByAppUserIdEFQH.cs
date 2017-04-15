using Bagombo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bagombo.EFCore;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bagombo.Data.Query.Queries;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetBlogPostsByAppUserIdEFQH : EFQHBase, IQueryHandlerAsync<GetBlogPostsByAppUserIdQuery, IEnumerable<BlogPost>>
  {
    public GetBlogPostsByAppUserIdEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<BlogPost>> HandleAsync(GetBlogPostsByAppUserIdQuery query)
    {
      var posts = await (from a in _context.Authors
                         where a.ApplicationUserId == query.AppUserId
                         join bp in _context.BlogPosts on a.Id equals bp.AuthorId
                         select bp).ToListAsync();

      return posts;
    }
  }
}

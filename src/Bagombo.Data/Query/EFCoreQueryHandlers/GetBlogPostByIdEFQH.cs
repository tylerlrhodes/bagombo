using Bagombo.Data.Query.Queries;
using Bagombo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bagombo.EFCore;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetBlogPostByIdEFQH : EFQHBase, IQueryHandlerAsync<GetBlogPostByIdQuery, BlogPost>
  {
    public GetBlogPostByIdEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<BlogPost> HandleAsync(GetBlogPostByIdQuery query)
    {
      return await _context.BlogPosts.Where(bp => bp.Id == query.Id).Include(bp => bp.Author).FirstOrDefaultAsync();
    }
  }
}

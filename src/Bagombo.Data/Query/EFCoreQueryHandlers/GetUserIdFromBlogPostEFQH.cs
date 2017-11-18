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
  public class GetUserIdFromBlogPostEFQH : EFQHBase, IQueryHandlerAsync<GetUserIdFromBlogPost, string>
  {
    public GetUserIdFromBlogPostEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<string> HandleAsync(GetUserIdFromBlogPost query)
    {
      var appUserId = await (from bp in _context.BlogPosts
                            where bp == query.blogpost
                            select bp.Author.ApplicationUserId).FirstOrDefaultAsync();

      return appUserId;
    }
  }
}

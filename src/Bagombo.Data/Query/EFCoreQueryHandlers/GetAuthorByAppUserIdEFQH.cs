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
  public class GetAuthorByAppUserIdEFQH : EFQHBase, IQueryHandlerAsync<GetAuthorByAppUserIdQuery, Author>
  {
    public GetAuthorByAppUserIdEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<Author> HandleAsync(GetAuthorByAppUserIdQuery query)
    {
      return await _context.Authors.AsNoTracking().Where(a => a.ApplicationUserId == query.Id).FirstOrDefaultAsync();
    }
  }
}

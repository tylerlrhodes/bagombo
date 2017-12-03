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
  public class GetAuthorByIdEFQH : EFQHBase, IQueryHandlerAsync<GetAuthorByIdQuery, Author>
  {
    public GetAuthorByIdEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<Author> HandleAsync(GetAuthorByIdQuery query)
    {
      return await _context.Authors.Where(a => a.Id == query.Id).FirstOrDefaultAsync();
    }
  }
}

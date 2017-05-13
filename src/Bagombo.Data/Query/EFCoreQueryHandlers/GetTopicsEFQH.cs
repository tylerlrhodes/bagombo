using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bagombo.Models;
using Bagombo.Data.Query.Queries;
using System.Threading.Tasks;
using Bagombo.EFCore;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetTopicsEFQH : EFQHBase, IQueryHandlerAsync<GetTopicsQuery, IEnumerable<Topic>>
  {
    public GetTopicsEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Topic>> HandleAsync(GetTopicsQuery query)
    {
      return await Task.FromResult(_context.Topics);
    }
  }
}

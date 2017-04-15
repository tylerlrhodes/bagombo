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
  public class GetFeaturesEFQH : EFQHBase, IQueryHandlerAsync<GetFeaturesQuery, IEnumerable<Feature>>
  {
    public GetFeaturesEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Feature>> HandleAsync(GetFeaturesQuery query)
    {
      return await Task.FromResult(_context.Features);
    }
  }
}

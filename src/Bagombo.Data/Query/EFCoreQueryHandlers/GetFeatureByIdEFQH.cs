using Bagombo.Data.Query.Queries;
using Bagombo.EFCore;
using Bagombo.Models;
using Bagombo.Models.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetFeatureByIdEFQH : EFQHBase, IQueryHandlerAsync<GetFeatureByIdQuery, Feature>
  {
    public GetFeatureByIdEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<Feature> HandleAsync(GetFeatureByIdQuery query)
    {
      var f = await _context.Features.FindAsync(query.Id);

      if (f != null)
      {
        return f;
      }
      else
      {
        return null;
      }
    }
  }
}

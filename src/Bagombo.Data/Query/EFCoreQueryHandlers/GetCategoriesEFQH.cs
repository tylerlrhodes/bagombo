using Bagombo.Data.Query.Queries;
using Bagombo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bagombo.EFCore;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetCategoriesEFQH : EFQHBase, IQueryHandlerAsync<GetCategoriesQuery, IEnumerable<Category>>
  {
    public GetCategoriesEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Category>> HandleAsync(GetCategoriesQuery query)
    {
      return await Task.FromResult(_context.Categories);
    }
  }
}

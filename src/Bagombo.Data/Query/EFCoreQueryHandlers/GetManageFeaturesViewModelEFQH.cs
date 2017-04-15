using Bagombo.Data.Query.Queries;
using Bagombo.EFCore;
using Bagombo.Models.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetManageFeaturesViewModelEFQH : EFQHBase, IQueryHandlerAsync<GetManageFeaturesViewModelQuery, ManageFeaturesViewModel>
  {
    public GetManageFeaturesViewModelEFQH(BlogDbContext context) : base(context) { }

    public async Task<ManageFeaturesViewModel> HandleAsync(GetManageFeaturesViewModelQuery query)
    {
      ManageFeaturesViewModel mfvm = new ManageFeaturesViewModel()
      {
        Features = await _context.Features.AsNoTracking().ToListAsync()
      };
      return mfvm;
    }
  }
}

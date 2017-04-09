using Bagombo.Data.Query.Queries;
using Bagombo.Models.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bagombo.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetManageCategoriesViewModelEFQH : EFQHBase, IQueryHandlerAsync<GetManageCategoriesViewModelQuery, ManageCategoriesViewModel>
  {
    public GetManageCategoriesViewModelEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<ManageCategoriesViewModel> HandleAsync(GetManageCategoriesViewModelQuery query)
    {
      ManageCategoriesViewModel amcvm = new ManageCategoriesViewModel();

      amcvm.Categories = await _context.Categories.AsNoTracking().ToListAsync();

      return amcvm;
    }
  }
}

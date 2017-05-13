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
  public class GetManageTopicsViewModelEFQH : EFQHBase, IQueryHandlerAsync<GetManageTopicsViewModelQuery, ManageTopicsViewModel>
  {
    public GetManageTopicsViewModelEFQH(BlogDbContext context) : base(context) { }

    public async Task<ManageTopicsViewModel> HandleAsync(GetManageTopicsViewModelQuery query)
    {
      ManageTopicsViewModel mfvm = new ManageTopicsViewModel()
      {
        Topics = await _context.Topics.AsNoTracking().ToListAsync()
      };
      return mfvm;
    }
  }
}

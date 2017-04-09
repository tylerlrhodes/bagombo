using Bagombo.Data.Query.Queries;
using Bagombo.EFCore;
using Bagombo.Models.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetFeatureByIdViewModelEFQH : IQueryHandlerAsync<GetFeatureByIdViewModelQuery, FeatureViewModel>
  {
    private BlogDbContext _context;

    public GetFeatureByIdViewModelEFQH(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<FeatureViewModel> HandleAsync(GetFeatureByIdViewModelQuery query)
    {
      var f = await _context.Features.FindAsync(query.Id);

      if (f != null)
      {
        return new FeatureViewModel()
        {
          Id = f.Id,
          Title = f.Title,
          Description = f.Description
        }; 
      }
      else
      {
        return null;
      }
    }
  }
}

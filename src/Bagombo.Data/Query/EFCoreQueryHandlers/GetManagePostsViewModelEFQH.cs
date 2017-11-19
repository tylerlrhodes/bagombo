using Bagombo.Data.Query.Queries;
using Bagombo.Models.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bagombo.EFCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Bagombo.Models;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetManagePostsViewModelEFQH : EFQHBase, IQueryHandlerAsync<GetManagePostsViewModelQuery, ManagePostsViewModel>
  {
    public GetManagePostsViewModelEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<ManagePostsViewModel> HandleAsync(GetManagePostsViewModelQuery query)
    {
      var mpvm = new ManagePostsViewModel()
      {
        posts = await PaginatedList<BlogPost>.CreateAsync(_context.BlogPosts.AsNoTracking()
          .OrderByDescending(bp => bp.ModifiedAt)
          .Include(a => a.Author), 
          query.CurrentPage,
          query.PageSize)
      };

      return mpvm;
    }
  }
}

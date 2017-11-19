using Bagombo.Models.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Query.Queries
{
  public class GetManagePostsViewModelQuery : IQuery<ManagePostsViewModel>
  {
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
  }
}

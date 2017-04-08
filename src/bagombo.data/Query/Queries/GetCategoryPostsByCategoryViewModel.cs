using System;
using System.Collections.Generic;
using System.Text;
using Bagombo.Models.ViewModels.Home;

namespace Bagombo.Data.Query.Queries
{
  public class GetCategoryPostsByCategoryViewModel : IQuery<CategoryPostsViewModel>
  {
    public long Id { get; set; }
  }
}

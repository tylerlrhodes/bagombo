using System;
using System.Collections.Generic;
using System.Text;
using blog.Models.ViewModels.Home;

namespace blog.data.Query.Queries
{
  public class GetViewCategoryPostsByCategory : IQuery<ViewCategoryPostsViewModel>
  {
    public long Id { get; set; }
  }
}

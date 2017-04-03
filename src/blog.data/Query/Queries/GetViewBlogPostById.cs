using blog.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace blog.data.Query.Queries
{
  public class GetViewBlogPostById : IQuery<ViewBlogPostViewModel>
  {
    public long Id { get; set; }
  }
}

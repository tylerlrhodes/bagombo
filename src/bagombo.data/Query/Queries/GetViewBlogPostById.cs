using Bagombo.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Query.Queries
{
  public class GetViewBlogPostById : IQuery<ViewBlogPostViewModel>
  {
    public long Id { get; set; }
  }
}

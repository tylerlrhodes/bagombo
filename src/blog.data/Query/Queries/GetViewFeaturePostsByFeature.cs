using blog.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace blog.data.Query.Queries
{
  public class GetViewFeaturePostsByFeature : IQuery<ViewFeaturePostsViewModel>
  {
    public long Id { get; set; }
  }
}

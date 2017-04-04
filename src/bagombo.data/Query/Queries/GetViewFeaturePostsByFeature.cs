using Bagombo.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.data.Query.Queries
{
  public class GetViewFeaturePostsByFeature : IQuery<ViewFeaturePostsViewModel>
  {
    public long Id { get; set; }
  }
}

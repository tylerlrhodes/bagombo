using Bagombo.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Query.Queries
{
  public class GetFeaturePostsByFeatureViewModelQuery : IQuery<FeaturePostsViewModel>
  {
    public long Id { get; set; }
  }
}

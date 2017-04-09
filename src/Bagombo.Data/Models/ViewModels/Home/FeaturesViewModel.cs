using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bagombo.Models.ViewModels.Home
{
  public class FeaturesViewModel
  {
    public IEnumerable<FeatureWithBlogCountViewModel> Features { get; set; }
  }
  public class FeatureWithBlogCountViewModel
  {
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int BlogCount { get; set; }
  }
}

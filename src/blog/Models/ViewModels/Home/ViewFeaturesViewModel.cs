using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog.Models.ViewModels.Home
{
  public class ViewFeaturesViewModel
  {
    public IEnumerable<FeatureViewModel> Features { get; set; }
  }
  public class FeatureViewModel
  {
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int BlogCount { get; set; }
  }
}

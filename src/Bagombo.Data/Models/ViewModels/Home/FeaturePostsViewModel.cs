using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bagombo.Models.ViewModels.Home
{
  public class FeaturePostsViewModel
  {
    public Feature Feature { get; set; }
    public IEnumerable<BlogPostViewModel> BlogPosts { get; set; }
  }
}

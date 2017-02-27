using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog.Models
{
  public class BlogPostFeature
  {
    public int BlogPostId { get; set; }
    public BlogPost BlogPost { get; set; }
    public int FeatureId { get; set; }
    public Feature Feature { get; set; }
    public int Order { get; set; }
  }
}

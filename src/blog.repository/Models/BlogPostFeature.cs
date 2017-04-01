using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog.Models
{
  public class BlogPostFeature
  {
    public long BlogPostId { get; set; }
    public BlogPost BlogPost { get; set; }
    public long FeatureId { get; set; }
    public Feature Feature { get; set; }
  }
}

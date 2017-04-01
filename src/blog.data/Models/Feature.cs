using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog.Models
{
  public class Feature
  {
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ICollection<BlogPostFeature> BlogPosts{ get; set; }
  }
}

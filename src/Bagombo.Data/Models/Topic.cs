using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bagombo.Models
{
  public class Topic
  {
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ICollection<BlogPostTopic> BlogPosts{ get; set; }
  }
}

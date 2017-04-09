using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bagombo.Models
{
  public class Category
  {
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<BlogPostCategory> BlogPosts { get; set; }
  }
}

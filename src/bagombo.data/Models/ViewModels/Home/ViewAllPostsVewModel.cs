using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bagombo.Models.ViewModels.Home
{
  public class ViewAllPostsViewModel
  {
    public int SortBy { get; set; }
    public IEnumerable<BlogPost> PostsByDate { get; set; }
    public IEnumerable<ViewPostsByCategory> Categories { get; set; }
  }
  public class ViewPostsByCategory
  {
    public Category Category { get; set; }
    public IEnumerable<BlogPost> Posts { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bagombo.Models.ViewModels.Home
{
  public class AllPostsViewModel
  {
    public int SortBy { get; set; }
    public IEnumerable<BlogPost> PostsByDate { get; set; }
    public IEnumerable<PostsByCategoryViewModel> Categories { get; set; }
  }
  public class PostsByCategoryViewModel
  {
    public Category Category { get; set; }
    public IEnumerable<BlogPost> Posts { get; set; }
  }
}

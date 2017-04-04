using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bagombo.Models.ViewModels.Home
{
  public class ViewCategoryPostsViewModel
  {
    public Category Category { get; set; }
    public IEnumerable<BlogPost> Posts { get; set; }
  }
}

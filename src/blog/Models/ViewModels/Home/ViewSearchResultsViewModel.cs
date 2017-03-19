using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog.Models.ViewModels.Home
{
  public class ViewSearchResultsViewModel
  {
    public string SearchTerm { get; set; }
    public IEnumerable<ViewSearchResultBlogPostViewModel> BlogPosts { get; set; }
  }
  public class ViewSearchResultBlogPostViewModel
  {
    public BlogPost BlogPost { get; set; }
    public IEnumerable<Category> Categories { get; set; }
  }
}

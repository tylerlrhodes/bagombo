using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bagombo.Models.ViewModels.Home
{
  public class SearchResultsViewModel
  {
    public string SearchTerm { get; set; }
    public IEnumerable<SearchResultBlogPostViewModel> BlogPosts { get; set; }
  }
  public class SearchResultBlogPostViewModel
  {
    public BlogPost BlogPost { get; set; }
    public IEnumerable<Category> Categories { get; set; }
  }
}

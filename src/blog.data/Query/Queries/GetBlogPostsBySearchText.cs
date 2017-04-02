using blog.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace blog.data.Query
{

  public class GetBlogPostsBySearchText : IQuery<IList<ViewSearchResultBlogPostViewModel>>
  {
    public string searchText { get; set; }
  }

}

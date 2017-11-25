using Bagombo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Query.Queries
{
  public class GetBlogPostsForHomePageQuery : IQuery<PaginatedList<BlogPost>>
  {
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
  }
}

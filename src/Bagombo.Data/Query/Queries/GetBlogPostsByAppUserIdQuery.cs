using System;
using System.Collections.Generic;
using System.Text;
using Bagombo.Models;


namespace Bagombo.Data.Query.Queries
{
  public class GetBlogPostsByAppUserIdQuery : IQuery<PaginatedList<BlogPost>>
  {
    public string AppUserId { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
  }
}

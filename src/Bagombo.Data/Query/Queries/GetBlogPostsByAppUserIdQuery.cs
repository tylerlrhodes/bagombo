using System;
using System.Collections.Generic;
using System.Text;
using Bagombo.Models;


namespace Bagombo.Data.Query.Queries
{
  public class GetBlogPostsByAppUserIdQuery : IQuery<IEnumerable<BlogPost>>
  {
    public string AppUserId { get; set; }
  }
}

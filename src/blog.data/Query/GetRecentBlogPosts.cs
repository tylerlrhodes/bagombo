using System;
using System.Collections.Generic;
using System.Text;
using blog.Models;

namespace blog.data.Query
{
  public class GetRecentBlogPosts : IQuery<IList<BlogPost>>
  {
    public int NumberOfPostsToGet { get; set; }
  }
}

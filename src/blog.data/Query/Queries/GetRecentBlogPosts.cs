using System;
using System.Collections.Generic;
using System.Text;
using blog.Models;
using blog.Models.ViewModels;
using blog.Models.ViewModels.Home;

namespace blog.data.Query
{
  public class GetRecentBlogPosts : IQuery<IList<BlogPost>>
  {
    public int NumberOfPostsToGet { get; set; }
  }

}

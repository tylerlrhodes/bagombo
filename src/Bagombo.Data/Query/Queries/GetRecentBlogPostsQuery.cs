using System;
using System.Collections.Generic;
using System.Text;
using Bagombo.Models;
using Bagombo.Models.ViewModels;
using Bagombo.Models.ViewModels.Home;

namespace Bagombo.Data.Query.Queries
{
  public class GetRecentBlogPostsQuery : IQuery<IList<BlogPost>>
  {
    public int NumberOfPostsToGet { get; set; }
  }

}

using Bagombo.Models;
using Bagombo.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Query.Queries
{
  public class GetBlogPostBySlugViewModelQuery : IQuery<BlogPostViewModel>
  {
    public string Slug { get; set; }
  }
}

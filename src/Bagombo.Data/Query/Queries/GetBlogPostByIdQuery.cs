using Bagombo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Query.Queries
{
  public class GetBlogPostByIdQuery : IQuery<BlogPost>
  {
    public long Id { get; set; }
  }
}

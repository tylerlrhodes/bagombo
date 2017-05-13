using System;
using System.Collections.Generic;
using System.Text;
using Bagombo.Models;

namespace Bagombo.Data.Query.Queries
{
  public class GetTopicsForBlogPostByIdQuery : IQuery<IEnumerable<Topic>>
  {
    public long Id { get; set; }
  }
}

using Bagombo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Query.Queries
{
  public class GetUserIdFromBlogPost : IQuery<string>
  {
    public BlogPost blogpost { get; set; }
  }
}

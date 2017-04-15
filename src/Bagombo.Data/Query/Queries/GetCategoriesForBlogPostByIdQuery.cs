using Bagombo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Query.Queries
{
  public class GetCategoriesForBlogPostByIdQuery : IQuery<IEnumerable<Category>>
  {
    public long Id { get; set; }
  }
}

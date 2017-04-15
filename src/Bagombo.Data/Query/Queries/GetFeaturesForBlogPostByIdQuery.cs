using System;
using System.Collections.Generic;
using System.Text;
using Bagombo.Models;

namespace Bagombo.Data.Query.Queries
{
  public class GetFeaturesForBlogPostByIdQuery : IQuery<IEnumerable<Feature>>
  {
    public long Id { get; set; }
  }
}

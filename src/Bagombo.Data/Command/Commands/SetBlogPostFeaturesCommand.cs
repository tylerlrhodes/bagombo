using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Command.Commands
{
  public class SetBlogPostFeaturesCommand
  {
    public long BlogPostId { get; set; }
    public IEnumerable<long> FeatureIds { get; set; }
  }
}

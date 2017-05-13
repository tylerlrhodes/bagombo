using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Command.Commands
{
  public class SetBlogPostTopicsCommand
  {
    public long BlogPostId { get; set; }
    public IEnumerable<long> TopicIds { get; set; }
  }
}

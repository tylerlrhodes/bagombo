using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Command.Commands
{
  public class SetBlogPostCategoriesByStringArrayCommand
  {
    public IList<string> Categories { get; set; }
    public long BlogPostId { get; set; }
  }
}

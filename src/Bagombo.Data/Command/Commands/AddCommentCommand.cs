using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Command.Commands
{
  public class AddCommentCommand
  {
    public long BlogPostId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
    public string Text { get; set; }
  }
}

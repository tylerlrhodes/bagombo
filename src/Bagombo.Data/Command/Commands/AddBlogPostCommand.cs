using Bagombo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Command.Commands
{
  public class AddBlogPostCommand
  {
    public Author Author { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public bool Public { get; set; }
    public DateTime PublishOn { get; set; }
    public string Image { get; set; }
    public long Id { get; set; }
    public bool IsHtml { get; set; } = false;
  }
}

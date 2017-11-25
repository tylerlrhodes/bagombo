using System;
using System.Collections.Generic;
using System.Text;
using Bagombo.Models;

namespace Bagombo.Data.Command.Commands
{
  public class UpdateBlogPostCommand
  {
    public long Id { get; set; }
    public Author NewAuthor { get; set; }
    public string NewTitle { get; set; }
    public string NewDescription { get; set; }
    public string NewContent { get; set; }
    public DateTime LastModifiedAt { get; set; }
    public DateTime NewPublishOn { get; set; }
    public bool NewPublic { get; set; }
    public string NewImage { get; set; }
  }
}

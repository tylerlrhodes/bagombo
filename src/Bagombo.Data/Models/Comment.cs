using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bagombo.Models
{
  public class Comment
  {
    public long Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Email { get; set; }

    public long BlogPostId { get; set; }
    public BlogPost BlogPost { get; set; }

    [Required]
    public string Text { get; set; }
  }
}

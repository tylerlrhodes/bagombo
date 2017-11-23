using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bagombo.Models
{
  public class Comment
  {
    public Comment()
    {
      Added = DateTime.Now;
    }

    public long Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Email { get; set; }
    public string WebSite { get; set; }
    [Required]
    public long BlogPostId { get; set; }
    public BlogPost BlogPost { get; set; }

    [Required]
    public string Text { get; set; }

    public DateTime Added { get; set; }
  }
}

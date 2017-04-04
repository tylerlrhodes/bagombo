using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Bagombo.Models.ViewModels.Author
{
  public class AddBlogPostViewModel
  {
    [Required]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }
    [Required]
    public string Description { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace blog.Models.ViewModels.Author
{
  public class EditBlogPostViewModel
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    [DisplayFormat(DataFormatString = "{0:MMMM dd yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
    public DateTime PublishOn { get; set; }
    [Required]
    public bool Public { get; set; }
  }
}

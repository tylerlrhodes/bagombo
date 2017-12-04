using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bagombo.Models.ViewModels.Home
{
  public class AddCommentViewModel
  {
    [Required]
    public long Id { get; set; }
    [Required]
    public string Slug { get; set; }
    [Required]
    public string Name { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
    [Required]
    public string Text { get; set; }
  }
}

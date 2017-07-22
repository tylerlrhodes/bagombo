using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bagombo.Models.ViewModels.Author
{
  public class ProfileViewModel
  {
    [Required]
    public long Id { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string Blurb { get; set; }

    public string Biography { get; set; }

    public string ImageLink { get; set; }
  }
}

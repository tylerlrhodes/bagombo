using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace blog.Models.ViewModels.Admin
{
  public class UserViewModel
  {
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
  }
}

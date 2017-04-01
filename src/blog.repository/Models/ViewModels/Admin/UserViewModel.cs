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
    [Display(Name = "User Name:")]
    [Required]
    public string UserName { get; set; }
    [Display(Name ="Email:")]
    [Required]
    public string Email { get; set; }
    [Display(Name = "Password:")]
    public string Password { get; set; }
    [Display(Name = "Is an author")]
    [Required]
    public bool IsAuthor { get; set; }
    [Display(Name = "First Name:")]
    public string FirstName { get; set; }
    [Display(Name = "Last Name:")]
    public string LastName { get; set; }
    public bool ExternalLogins { get; set; }
    [Display(Name = "Reset Password?")]
    [Required]
    public bool ChangePassword { get; set; }
  }
}

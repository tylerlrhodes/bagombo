using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bagombo.Models
{
  public class ApplicationUser : IdentityUser
  {
    [NotMapped]
    public Author Author { get; set; }
  }
}

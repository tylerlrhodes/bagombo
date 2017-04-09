using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Bagombo.Models
{
  public class ApplicationUser : IdentityUser
  {
    public Author Author { get; set; }
  }
}

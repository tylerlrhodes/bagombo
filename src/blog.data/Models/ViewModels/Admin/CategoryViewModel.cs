using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace blog.Models.ViewModels.Admin
{
  public class CategoryViewModel
  {
    [Required]
    public long Id { get; set; }

    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Description { get; set; }
  }
}

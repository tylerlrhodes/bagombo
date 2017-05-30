using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bagombo.Models
{
  public class Author
  {
    public long Id { get; set; }
    public string ApplicationUserId { get; set; }
    [NotMapped]
    public ApplicationUser ApplicationUser { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    //public string Blurb { get; set; }
    public string Biography { get; set; }
    //public string ImageLink { get; set; }
    public virtual ICollection<BlogPost> BlogPosts { get; set; }
  }
}

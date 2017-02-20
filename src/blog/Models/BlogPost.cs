using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog.Models
{
  public class BlogPost
  {
    public int Id { get; set; }
    public string AuthorId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
  }
}

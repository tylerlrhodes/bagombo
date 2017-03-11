using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog.Models
{
  public class BlogPost
  {
    public int Id { get; set; }
    public int? AuthorId { get; set; }
    public Author Author { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public DateTime PublishOn { get; set; }
    public bool Public { get; set; }
    public ICollection<BlogPostCategory> Categories { get; set; }
    public ICollection<BlogPostFeature> Features { get; set; }
  }
}

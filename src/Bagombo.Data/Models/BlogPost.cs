using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bagombo.Models
{
  public class BlogPost
  {
    public long Id { get; set; }
    public long? AuthorId { get; set; }
    public Author Author { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public DateTime PublishOn { get; set; }
    public bool Public { get; set; }
    public ICollection<BlogPostCategory> BlogPostCategory { get; set; }
    public ICollection<BlogPostTopic> BlogPostTopic { get; set; }
  }
}

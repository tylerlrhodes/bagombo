using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bagombo.Models;

namespace Bagombo.Models.ViewModels.Home
{
  public class BlogPostViewModel
  {
    public long Id { get; set; }
    public string Slug { get; set; }
    public string Title { get; set; }
    public Bagombo.Models.Author Author { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public DateTime ModifiedAt { get; set; }
    public DateTime PublishOn { get; set; }
    public IEnumerable<Category> Categories { get; set; }
    public IEnumerable<Topic> Topics { get; set; }
    public IEnumerable<Comment> Comments { get; set; }
    public string Image { get; set; }
  }
}

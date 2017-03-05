using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog.Models.ViewModels.Home
{
  public class ViewBlogPostViewModel
  {
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public DateTime ModifiedAt { get; set; }
  }
}

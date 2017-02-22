using blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog.ViewModels.Admin
{
  public class ListPostsViewModel
  {
    public IEnumerable<BlogPost> posts;
  }
}

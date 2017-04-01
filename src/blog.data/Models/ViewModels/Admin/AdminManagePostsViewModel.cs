using blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog.Models.ViewModels.Admin
{
  public class AdminManagePostsViewModel
  {
    public IEnumerable<BlogPost> posts;
  }
}

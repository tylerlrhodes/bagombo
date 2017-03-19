using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blog.Models.ViewModels.Home
{
  public class ViewHomeViewModel
  {
    public IEnumerable<BlogPost> RecentPosts { get; set; }
  }
}

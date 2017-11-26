using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bagombo.Models.ViewModels.Home
{
  public class HomeViewModel
  {
    public IEnumerable<Category> Categories { get; set; }
    public PaginatedList<BlogPost> RecentPosts { get; set; }
  }
}

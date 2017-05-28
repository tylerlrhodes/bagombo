using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bagombo.Models.ViewModels.Home
{
  public class HomeViewModel
  {
    public IEnumerable<BlogPost> RecentPosts { get; set; }
    public TopicPostsViewModel TopicPosts { get; set; }
  }
}

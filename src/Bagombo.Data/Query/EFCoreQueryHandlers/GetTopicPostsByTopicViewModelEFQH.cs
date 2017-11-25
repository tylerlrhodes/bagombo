using Bagombo.Data.Query.Queries;
using Bagombo.EFCore;
using Bagombo.Models.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  class GetTopicPostsByTopicViewModelEFQH : EFQHBase, IQueryHandlerAsync<GetTopicPostsByTopicViewModelQuery, TopicPostsViewModel>
  {
    public GetTopicPostsByTopicViewModelEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<TopicPostsViewModel> HandleAsync(GetTopicPostsByTopicViewModelQuery query)
    {

      var topic = await _context.Topics.FindAsync(query.Id);

      if (topic == null)
      {
        return null;
      }

      var bpfs = await _context.BlogPostTopic
                              .AsNoTracking()
                              .Where(bpf => bpf.TopicId == topic.Id && bpf.BlogPost.Public == true && bpf.BlogPost.PublishOn < DateTime.Now)
                              .Include(bpf => bpf.BlogPost)
                                .ThenInclude(bp => bp.Author)
                              .Include(bpf => bpf.BlogPost)
                                .ThenInclude(bp => bp.Comments)
                              .Include(bpf => bpf.BlogPost)
                                .ThenInclude(bp => bp.BlogPostCategory)
                                .ThenInclude(bpc => bpc.Category)
                              .OrderByDescending(bp => bp.BlogPost.PublishOn)
                              .ThenByDescending(bp => bp.BlogPost.ModifiedAt)
                              .ToListAsync();

      List<BlogPostViewModel> viewPosts = new List<BlogPostViewModel>();

      foreach (var bpf in bpfs)
      {
        var bpView = new BlogPostViewModel()
        {
          Author = bpf.BlogPost.Author,
          Title = bpf.BlogPost.Title,
          Description = bpf.BlogPost.Description,
          Categories = bpf.BlogPost.BlogPostCategory.Select(c => c.Category).ToList(),
          ModifiedAt = bpf.BlogPost.ModifiedAt,
          Id = bpf.BlogPost.Id,
          Content = bpf.BlogPost.Content,
          Comments = bpf.BlogPost.Comments
        };
        viewPosts.Add(bpView);
      }

      return new TopicPostsViewModel()
      {
        Topic = topic,
        BlogPosts = viewPosts
      };

    }

  }

}

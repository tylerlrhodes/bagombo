using Bagombo.Data.Query.Queries;
using Bagombo.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bagombo.EFCore;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bagombo.Models;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  class GetTopicPostsForHomePageViewModelEFQH : EFQHBase, IQueryHandlerAsync<GetTopicPostsForHomePageViewModelQuery, TopicPostsViewModel>
  {
    public GetTopicPostsForHomePageViewModelEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<TopicPostsViewModel> HandleAsync(GetTopicPostsForHomePageViewModelQuery query)
    {
      var topic = await _context.Topics.Where(t => t.ShowOnHomePage == true).FirstOrDefaultAsync();

      if (topic != null)
      {
        var posts = await _context.BlogPostTopic.AsNoTracking()
                                                .Where(t => t.TopicId == topic.Id && t.BlogPost.Public == true && t.BlogPost.PublishOn <= DateTime.Now)
                                                //.Include(t => t.BlogPost)
                                                .OrderByDescending(t => t.BlogPost.PublishOn)
                                                .ThenByDescending(t => t.BlogPost.ModifiedAt)
                                                //  .ThenInclude(bp => bp.Author)
                                                //.Include(t => t.BlogPost)
                                                //  .ThenInclude(bp => bp.BlogPostCategory)
                                                //.ThenInclude(bpc => bpc.Category)
                                                .Select(t => new BlogPostViewModel
                                                {
                                                  Title = t.BlogPost.Title,
                                                  Id = t.BlogPost.Id,
                                                  Slug = t.BlogPost.Slug,
                                                  Description = t.BlogPost.Description,
                                                  Author = t.BlogPost.Author,
                                                  ModifiedAt = t.BlogPost.ModifiedAt,
                                                  PublishOn = t.BlogPost.PublishOn,
                                                  Content = t.BlogPost.Content,
                                                  Categories = t.BlogPost.BlogPostCategory.Select(c => c.Category).ToList()
                                                }).ToListAsync();

        return new TopicPostsViewModel
        {
          Topic = topic,
          BlogPosts = posts
        };
      }
      else
      {
        return new TopicPostsViewModel
        {
          Topic = null,
          BlogPosts = new List<BlogPostViewModel>()
        };
      }
    }
  }
}

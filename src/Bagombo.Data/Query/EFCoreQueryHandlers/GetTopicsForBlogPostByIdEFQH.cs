using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bagombo.Models;
using Bagombo.Data.Query.Queries;
using System.Threading.Tasks;
using Bagombo.EFCore;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetTopicForBlogPostByIdEFQH : EFQHBase, IQueryHandlerAsync<GetTopicsForBlogPostByIdQuery, IEnumerable<Topic>>
  {
    public GetTopicForBlogPostByIdEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Topic>> HandleAsync(GetTopicsForBlogPostByIdQuery query)
    {
      return await (from bpf in _context.BlogPostTopic
                    where bpf.BlogPostId == query.Id
                    join f in _context.Topics on bpf.TopicId equals f.Id
                    select f).ToListAsync();
    }
  }
}

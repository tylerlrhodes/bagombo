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
  public class GetFeaturesForBlogPostByIdEFQH : EFQHBase, IQueryHandlerAsync<GetFeaturesForBlogPostByIdQuery, IEnumerable<Feature>>
  {
    public GetFeaturesForBlogPostByIdEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Feature>> HandleAsync(GetFeaturesForBlogPostByIdQuery query)
    {
      return await (from bpf in _context.BlogPostFeature
                    where bpf.BlogPostId == query.Id
                    join f in _context.Features on bpf.FeatureId equals f.Id
                    select f).ToListAsync();
    }
  }
}

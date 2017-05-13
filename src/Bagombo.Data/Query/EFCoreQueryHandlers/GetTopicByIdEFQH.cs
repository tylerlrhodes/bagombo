using Bagombo.Data.Query.Queries;
using Bagombo.EFCore;
using Bagombo.Models;
using Bagombo.Models.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetTopicByIdEFQH : EFQHBase, IQueryHandlerAsync<GetTopicByIdQuery, Topic>
  {
    public GetTopicByIdEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<Topic> HandleAsync(GetTopicByIdQuery query)
    {
      var f = await _context.Topics.FindAsync(query.Id);

      if (f != null)
      {
        return f;
      }
      else
      {
        return null;
      }
    }
  }
}

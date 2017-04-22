using Bagombo.Data.Command.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bagombo.EFCore;
using Bagombo.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  public class SetBlogPostFeaturesCommandEFCH : EFCHBase, ICommandHandlerAsync<SetBlogPostFeaturesCommand>
  {
    public SetBlogPostFeaturesCommandEFCH(BlogDbContext context) : base(context)
    {
    }

    public async Task<CommandResult<SetBlogPostFeaturesCommand>> ExecuteAsync(SetBlogPostFeaturesCommand command)
    {
      try
      {
        var bpfts = await _context.BlogPostFeature.Where(bpf => bpf.BlogPostId == command.BlogPostId).ToListAsync();

        _context.BlogPostFeature.RemoveRange(bpfts);

        await _context.SaveChangesAsync();

        foreach (long fid in command.FeatureIds)
        {
          BlogPostFeature bpf = new BlogPostFeature
          {
            BlogPostId = command.BlogPostId,
            FeatureId = fid
          };

          _context.BlogPostFeature.Add(bpf);
        }

        await _context.SaveChangesAsync();

        return new CommandResult<SetBlogPostFeaturesCommand>(command, true);
      }
      catch (Exception ex)
      {
        return new CommandResult<SetBlogPostFeaturesCommand>(command, false, ex);
      }
    }
  }
}

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
  public class SetBlogPostTopicsCommandEFCH : EFCHBase, ICommandHandlerAsync<SetBlogPostTopicsCommand>
  {
    public SetBlogPostTopicsCommandEFCH(BlogDbContext context) : base(context)
    {
    }

    public async Task<CommandResult<SetBlogPostTopicsCommand>> ExecuteAsync(SetBlogPostTopicsCommand command)
    {
      try
      {
        var bpfts = await _context.BlogPostTopic.Where(bpf => bpf.BlogPostId == command.BlogPostId).ToListAsync();

        _context.BlogPostTopic.RemoveRange(bpfts);

        await _context.SaveChangesAsync();

        foreach (long fid in command.TopicIds)
        {
          BlogPostTopic bpf = new BlogPostTopic
          {
            BlogPostId = command.BlogPostId,
            TopicId = fid
          };

          _context.BlogPostTopic.Add(bpf);
        }

        await _context.SaveChangesAsync();

        return new CommandResult<SetBlogPostTopicsCommand>(command, true);
      }
      catch (Exception ex)
      {
        return new CommandResult<SetBlogPostTopicsCommand>(command, false, ex);
      }
    }
  }
}

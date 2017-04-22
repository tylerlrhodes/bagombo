using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bagombo.Data.Command.Commands;
using Bagombo.Models;
using Bagombo.EFCore;
using System.Threading.Tasks;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  public class DeleteBlogPostCommandEFCH : EFCHBase, ICommandHandlerAsync<DeleteBlogPostCommand>
  {
    public DeleteBlogPostCommandEFCH(BlogDbContext context) : base(context)
    {
    }

    public async Task<CommandResult<DeleteBlogPostCommand>> ExecuteAsync(DeleteBlogPostCommand command)
    {
      try
      {
        BlogPost toRemove = await _context.BlogPosts.FindAsync(command.Id);

        if (toRemove != null)
        {
          _context.BlogPosts.Remove(toRemove);

          await _context.SaveChangesAsync();

          return new CommandResult<DeleteBlogPostCommand>(command, true);
        }
        else
        {
          return new CommandResult<DeleteBlogPostCommand>(command, false);
        }
      }
      catch (Exception ex)
      {
        return new CommandResult<DeleteBlogPostCommand>(command, false, ex);
      }
    }
  }
}

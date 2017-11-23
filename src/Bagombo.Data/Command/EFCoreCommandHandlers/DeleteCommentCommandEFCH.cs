using Bagombo.Data.Command.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bagombo.EFCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  public class DeleteCommentCommandEFCH : EFCHBase, ICommandHandlerAsync<DeleteCommentCommand>
  {
    public DeleteCommentCommandEFCH(BlogDbContext context) : base(context)
    {
    }

    public async Task<CommandResult<DeleteCommentCommand>> ExecuteAsync(DeleteCommentCommand command)
    {
      var comment = await _context.Comments.Where(c => c.Id == command.Id).FirstOrDefaultAsync();

      if (comment == null)
      {
        return new CommandResult<DeleteCommentCommand>(command, true);
      }
      else
      {
        try
        {
          _context.Comments.Remove(comment);

          await _context.SaveChangesAsync();

          return new CommandResult<DeleteCommentCommand>(command, true);
        }
        catch (Exception ex)
        {
          return new CommandResult<DeleteCommentCommand>(command, false, ex);
        }
      }
    }
  }
}

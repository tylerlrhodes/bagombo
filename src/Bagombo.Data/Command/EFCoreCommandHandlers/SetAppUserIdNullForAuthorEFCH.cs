using Bagombo.Data.Command.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bagombo.EFCore;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  public class SetAppUserIdNullForAuthorEFCH : EFCHBase, ICommandHandlerAsync<SetAppUserIdNullForAuthorCommand>
  {
    public SetAppUserIdNullForAuthorEFCH(BlogDbContext context) : base(context)
    {
    }

    public async Task<CommandResult<SetAppUserIdNullForAuthorCommand>> ExecuteAsync(SetAppUserIdNullForAuthorCommand command)
    {
      try
      {
        var Author = await _context.Authors.Where(a => a.ApplicationUserId == command.Id).FirstOrDefaultAsync();

        Author.ApplicationUser = null;
        Author.ApplicationUserId = null;

        await _context.SaveChangesAsync();

        return new CommandResult<SetAppUserIdNullForAuthorCommand>(command, true);
      }
      catch (Exception)
      {
        return new CommandResult<SetAppUserIdNullForAuthorCommand>(command, false);
      }
    }
  }
}

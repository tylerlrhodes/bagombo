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
  public class UpdateAuthorCommandEFCH : EFCHBase, ICommandHandlerAsync<UpdateAuthorCommand>
  {
    public UpdateAuthorCommandEFCH(BlogDbContext context) : base(context)
    {
    }

    public async Task<CommandResult<UpdateAuthorCommand>> ExecuteAsync(UpdateAuthorCommand command)
    {
      try
      {
        var author = await _context.Authors.Where(a => a.Id == command.Id).FirstOrDefaultAsync();

        author.FirstName = command.NewFirstName;
        author.LastName = command.NewLastName;

        await _context.SaveChangesAsync();

        return new CommandResult<UpdateAuthorCommand>(command, true);
      }
      catch (Exception ex)
      {
        return new CommandResult<UpdateAuthorCommand>(command, false, ex);
      }
    }
  }
}

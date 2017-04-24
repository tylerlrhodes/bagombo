using Bagombo.Data.Command.Commands;
using Bagombo.EFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  public class EditFeatureCommandEFCH : EFCHBase, ICommandHandlerAsync<EditFeatureCommand>
  {
    public EditFeatureCommandEFCH(BlogDbContext context) : base(context) { }

    public async Task<CommandResult<EditFeatureCommand>> ExecuteAsync(EditFeatureCommand command)
    {
      try
      {
        var f = await _context.Features.FindAsync(command.Id);

        f.Title = command.NewTitle;
        f.Description = command.NewDescription;

        await _context.SaveChangesAsync();

        return new CommandResult<EditFeatureCommand>(command, true);
      }
      catch (Exception ex)
      {
        return new CommandResult<EditFeatureCommand>(command, false, ex);
      }
    }
  }
}

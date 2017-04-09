using Bagombo.Data.Command.Commands;
using Bagombo.EFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  public class DeleteFeatureCommandEFCH : EFCHBase, ICommandHandlerAsync<DeleteFeatureCommand>
  {
    public DeleteFeatureCommandEFCH(BlogDbContext context) : base(context) { }

    public async Task<CommandResult<DeleteFeatureCommand>> ExecuteAsync(DeleteFeatureCommand command)
    {
      try
      {
        var feature = await _context.Features.FindAsync(command.Id);

        _context.Features.Remove(feature);

        await _context.SaveChangesAsync();

        return new CommandResult<DeleteFeatureCommand>(command, true);
      }
      catch (Exception)
      {
        return new CommandResult<DeleteFeatureCommand>(command, false);
      }
    }
  }
}

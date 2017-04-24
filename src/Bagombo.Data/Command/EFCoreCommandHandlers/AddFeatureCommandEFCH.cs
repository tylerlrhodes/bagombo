using System;
using System.Collections.Generic;
using System.Text;
using Bagombo.Models;
using Bagombo.Data.Command.Commands;
using Bagombo.EFCore;
using System.Threading.Tasks;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  public class AddFeatureCommandEFCH : EFCHBase, ICommandHandlerAsync<AddFeatureCommand>
  {
    public AddFeatureCommandEFCH(BlogDbContext context) : base(context)
    {
    }

    public async Task<CommandResult<AddFeatureCommand>> ExecuteAsync(AddFeatureCommand command)
    {
      try
      {
        _context.Features.Add(command.Feature);
        await _context.SaveChangesAsync();
        return new CommandResult<AddFeatureCommand>(command, true);
      }
      catch (Exception ex)
      {
        return new CommandResult<AddFeatureCommand>(command, false, ex);
      }
    }
  }
}

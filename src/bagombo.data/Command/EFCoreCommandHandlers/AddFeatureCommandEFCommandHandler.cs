using System;
using System.Collections.Generic;
using System.Text;
using Bagombo.Models;
using Bagombo.Data.Command.Commands;
using Bagombo.EFCore;
using System.Threading.Tasks;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  public class AddFeatureCommandEFCommandHandler : ICommandHandlerAsync<AddFeatureCommand>
  {
    private BlogDbContext _context;

    public AddFeatureCommandEFCommandHandler(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<CommandResult<AddFeatureCommand>> ExecuteAsync(AddFeatureCommand command)
    {
      try
      {
        _context.Features.Add(command.Feature);
        await _context.SaveChangesAsync();
        return new CommandResult<AddFeatureCommand>(command, true);
      }
      catch (Exception)
      {
        return new CommandResult<AddFeatureCommand>(command, false);
      }
    }
  }
}

using System;
using System.Collections.Generic;
using System.Text;
using Bagombo.Models;
using Bagombo.Data.Command.Commands;
using Bagombo.EFCore;
using System.Threading.Tasks;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  public class AddTopicCommandEFCH : EFCHBase, ICommandHandlerAsync<AddTopicCommand>
  {
    public AddTopicCommandEFCH(BlogDbContext context) : base(context)
    {
    }

    public async Task<CommandResult<AddTopicCommand>> ExecuteAsync(AddTopicCommand command)
    {
      try
      {
        _context.Topic.Add(command.Topic);
        await _context.SaveChangesAsync();
        return new CommandResult<AddTopicCommand>(command, true);
      }
      catch (Exception ex)
      {
        return new CommandResult<AddTopicCommand>(command, false, ex);
      }
    }
  }
}

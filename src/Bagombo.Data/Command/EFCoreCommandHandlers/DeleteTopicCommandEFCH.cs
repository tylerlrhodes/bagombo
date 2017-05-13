using Bagombo.Data.Command.Commands;
using Bagombo.EFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  public class DeleteTopicCommandEFCH : EFCHBase, ICommandHandlerAsync<DeleteTopicCommand>
  {
    public DeleteTopicCommandEFCH(BlogDbContext context) : base(context) { }

    public async Task<CommandResult<DeleteTopicCommand>> ExecuteAsync(DeleteTopicCommand command)
    {
      try
      {
        var feature = await _context.Topic.FindAsync(command.Id);

        _context.Topic.Remove(feature);

        await _context.SaveChangesAsync();

        return new CommandResult<DeleteTopicCommand>(command, true);
      }
      catch (Exception ex)
      {
        return new CommandResult<DeleteTopicCommand>(command, false, ex);
      }
    }
  }
}

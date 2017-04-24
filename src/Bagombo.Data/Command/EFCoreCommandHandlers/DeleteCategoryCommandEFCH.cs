using Bagombo.Data.Command.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bagombo.EFCore;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  public class DeleteCategoryCommandEFCH : EFCHBase, ICommandHandlerAsync<DeleteCategoryCommand>
  {
    public DeleteCategoryCommandEFCH(BlogDbContext context) : base(context)
    {
    }

    public async Task<CommandResult<DeleteCategoryCommand>> ExecuteAsync(DeleteCategoryCommand command)
    {
      try
      {
        var c = await _context.Categories.FindAsync(command.Id);

        _context.Categories.Remove(c);

        await _context.SaveChangesAsync();

        return new CommandResult<DeleteCategoryCommand>(command, true);

      }
      catch (Exception ex)
      {
        return new CommandResult<DeleteCategoryCommand>(command, false, ex);
      }
    }
  }
}

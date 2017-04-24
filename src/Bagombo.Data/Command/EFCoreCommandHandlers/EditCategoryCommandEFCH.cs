using Bagombo.Data.Command.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bagombo.EFCore;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  public class EditCategoryCommandEFCH : EFCHBase, ICommandHandlerAsync<EditCategoryCommand>
  {
    public EditCategoryCommandEFCH(BlogDbContext context) : base(context)
    {
    }

    public async Task<CommandResult<EditCategoryCommand>> ExecuteAsync(EditCategoryCommand command)
    {
      try
      {
        var c = await _context.Categories.FindAsync(command.Id);

        c.Name = command.NewName;
        c.Description = command.NewDescription;

        await _context.SaveChangesAsync();

        return new CommandResult<EditCategoryCommand>(command, true);
      }
      catch (Exception ex)
      {
        return new CommandResult<EditCategoryCommand>(command, false, ex);
      }
    }
  }
}

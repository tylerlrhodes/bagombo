using Bagombo.Data.Command.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bagombo.EFCore;
using Bagombo.Models;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  public class AddCategoryCommandEFCH : EFCHBase, ICommandHandlerAsync<AddCategoryCommand>
  {
    public AddCategoryCommandEFCH(BlogDbContext context) : base(context)
    {
    }

    public async Task<CommandResult<AddCategoryCommand>> ExecuteAsync(AddCategoryCommand command)
    {
      try
      {
        var c = new Category()
        {
          Name = command.Name,
          Description = command.Description
        };

        _context.Categories.Add(c);

        await _context.SaveChangesAsync();

        command.Id = c.Id;

        return new CommandResult<AddCategoryCommand>(command, true);
      }
      catch (Exception ex)
      {
        return new CommandResult<AddCategoryCommand>(command, false, ex);
      }
    }
  }
}

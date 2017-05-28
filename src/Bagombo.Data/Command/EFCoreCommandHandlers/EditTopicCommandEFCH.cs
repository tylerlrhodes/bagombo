using Bagombo.Data.Command.Commands;
using Bagombo.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  public class EditTopicCommandEFCH : EFCHBase, ICommandHandlerAsync<EditTopicCommand>
  {
    public EditTopicCommandEFCH(BlogDbContext context) : base(context) { }

    public async Task<CommandResult<EditTopicCommand>> ExecuteAsync(EditTopicCommand command)
    {
      try
      {
        if (command.NewShowOnHomePage == true)
        {
          var curTopicOnHomePage = await _context.Topics.Where(t => t.ShowOnHomePage == true).FirstOrDefaultAsync();

          if (curTopicOnHomePage != null)
          {
            curTopicOnHomePage.ShowOnHomePage = false;

            await _context.SaveChangesAsync();
          }
        }

        var f = await _context.Topics.FindAsync(command.Id);

        f.Title = command.NewTitle;
        f.Description = command.NewDescription;
        f.ShowOnHomePage = command.NewShowOnHomePage;

        await _context.SaveChangesAsync();

        return new CommandResult<EditTopicCommand>(command, true);
      }
      catch (Exception ex)
      {
        return new CommandResult<EditTopicCommand>(command, false, ex);
      }
    }
  }
}

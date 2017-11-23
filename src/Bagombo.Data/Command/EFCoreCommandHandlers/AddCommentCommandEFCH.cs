using Bagombo.Data.Command.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bagombo.EFCore;
using Bagombo.Models;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  class AddCommentCommandEFCH : EFCHBase, ICommandHandlerAsync<AddCommentCommand>
  {
    public AddCommentCommandEFCH(BlogDbContext context) : base(context)
    {
    }

    public async Task<CommandResult<AddCommentCommand>> ExecuteAsync(AddCommentCommand command)
    {
      if (command.BlogPostId == default(int) ||
          string.IsNullOrEmpty(command.Name) ||
          string.IsNullOrEmpty(command.Text))
      {
        return new CommandResult<AddCommentCommand>(command, false, new Exception("Error adding comment, not all necessary values present"));
      }

      var comment = new Comment()
      {
        BlogPostId = command.BlogPostId,
        Name = command.Name,
        Email = command.Email,
        WebSite = command.Website,
        Text = command.Text,
      };

      _context.Comments.Add(comment);

      try
      {
        await _context.SaveChangesAsync();
        return new CommandResult<AddCommentCommand>(command, true);
      }
      catch (Exception ex)
      {
        return new CommandResult<AddCommentCommand>(command, false, ex);
      }
    }
  }
}

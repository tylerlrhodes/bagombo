using System;
using System.Collections.Generic;
using System.Text;
using Bagombo.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bagombo.Data.Command.Commands;
using System.Threading.Tasks;
using Bagombo.EFCore;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  public class UpdateBlogPostCommandEFCH : EFCHBase, ICommandHandlerAsync<UpdateBlogPostCommand>
  {
    public UpdateBlogPostCommandEFCH(BlogDbContext context) : base(context)
    {
    }

    public async Task<CommandResult<UpdateBlogPostCommand>> ExecuteAsync(UpdateBlogPostCommand command)
    {
      try
      {
        var post = await _context.BlogPosts.Where(bp => bp.Id == command.Id).FirstOrDefaultAsync();

        if (post != null)
        {
          post.Author = command.NewAuthor;
          post.Title = command.NewTitle;
          post.Description = command.NewDescription;
          post.Content = command.NewContent;
          post.ModifiedAt = command.LastModifiedAt;
          post.PublishOn = command.NewPublishOn;
          post.Public = command.NewPublic;

          await _context.SaveChangesAsync();

          return new CommandResult<UpdateBlogPostCommand>(command, true);
        }
        else
        {
          return new CommandResult<UpdateBlogPostCommand>(command, false);
        }
      }
      catch (Exception ex)
      {
        return new CommandResult<UpdateBlogPostCommand>(command, false, ex);
      }
    }
  }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bagombo.Data.Command.Commands;
using System.Threading.Tasks;
using Bagombo.EFCore;
using Bagombo.Models;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  public class SetBlogPostCategoriesByStringArrayEFCH : EFCHBase, ICommandHandlerAsync<SetBlogPostCategoriesByStringArrayCommand>
  {
    public SetBlogPostCategoriesByStringArrayEFCH(BlogDbContext context) : base(context)
    {
    }

    public async Task<CommandResult<SetBlogPostCategoriesByStringArrayCommand>> ExecuteAsync(SetBlogPostCategoriesByStringArrayCommand command)
    {
      if (command.Categories.Count() == 0)
      {
        return await Task.FromResult(new CommandResult<SetBlogPostCategoriesByStringArrayCommand>(command, true));
      }
      else
      {
        try { 
          foreach (var category in command.Categories)
          {
            // find the category
            var cat = await _context.Categories.Where(c => c.Name.ToLower() == category).FirstOrDefaultAsync();

            if (cat != null)
            {
              var bpc = new BlogPostCategory
              {
                BlogPostId = command.BlogPostId,
                CategoryId = cat.Id
              };

              _context.BlogPostCategory.Add(bpc);
            }
          }
          await _context.SaveChangesAsync();

          return new CommandResult<SetBlogPostCategoriesByStringArrayCommand>(command, true);
        }
        catch (Exception ex)
        {
          return new CommandResult<SetBlogPostCategoriesByStringArrayCommand>(command, false, ex);
        }
      }
    }
  }
}

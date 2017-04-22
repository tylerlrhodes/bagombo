using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bagombo.Data.Command.Commands;
using Bagombo.Models;
using System.Threading.Tasks;
using Bagombo.EFCore;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  public class SetBlogPostCategoriesCommandEFCH : EFCHBase, ICommandHandlerAsync<SetBlogPostCategoriesCommand>
  {
    public SetBlogPostCategoriesCommandEFCH(BlogDbContext context) : base(context)
    {
    }

    public async Task<CommandResult<SetBlogPostCategoriesCommand>> ExecuteAsync(SetBlogPostCategoriesCommand command)
    {
      try
      {
        var bpcts = await _context.BlogPostCategory.Where(bpc => bpc.BlogPostId == command.BlogPostId).ToListAsync();

        _context.BlogPostCategory.RemoveRange(bpcts);

        await _context.SaveChangesAsync();

        foreach (long cid in command.CategoryIds)
        {
          BlogPostCategory bpc = new BlogPostCategory
          {
            BlogPostId = command.BlogPostId,
            CategoryId = cid
          };

          _context.BlogPostCategory.Add(bpc);
        }

        await _context.SaveChangesAsync();

        return new CommandResult<SetBlogPostCategoriesCommand>(command, true);
      }
      catch (Exception ex)
      {
        return new CommandResult<SetBlogPostCategoriesCommand>(command, false, ex);
      }
    }
  }
}

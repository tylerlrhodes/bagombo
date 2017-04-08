using Bagombo.Data.Query.Queries;
using Bagombo.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bagombo.EFCore;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetBlogPostByIdViewModelEFQueryHandler : IQueryHandlerAsync<GetBlogPostByIdViewModel, BlogPostViewModel>
  {
    BlogDbContext _context;

    public GetBlogPostByIdViewModelEFQueryHandler(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<BlogPostViewModel> HandleAsync(GetBlogPostByIdViewModel query)
    {
      var post = await _context.BlogPosts
                          .AsNoTracking()
                          .Include(bp => bp.Author)
                          .Include(bp => bp.BlogPostCategory)
                            .ThenInclude(bpc => bpc.Category)
                          .Where(bp => bp.Id == query.Id && bp.Public == true && bp.PublishOn < DateTime.Now)
                          .FirstOrDefaultAsync();

      if (post == null)
        return null;

      var bpvm = new BlogPostViewModel()
      {
        Author = $"{post.Author.FirstName} {post.Author.LastName}",
        Title = post.Title,
        Description = post.Description,
        Content = post.Content,
        ModifiedAt = post.ModifiedAt,
        Categories = post.BlogPostCategory.Select(c => c.Category).ToList()
      };

      return bpvm;
    }
  }
}

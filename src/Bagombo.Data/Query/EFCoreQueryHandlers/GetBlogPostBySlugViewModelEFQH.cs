using Bagombo.Data.Query.Queries;
using Bagombo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bagombo.EFCore;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bagombo.Models.ViewModels.Home;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetBlogPostBySlugViewModelEFQH : EFQHBase, IQueryHandlerAsync<GetBlogPostBySlugViewModelQuery, BlogPostViewModel>
  {
    public GetBlogPostBySlugViewModelEFQH(BlogDbContext context) : base(context)
    {
    }

    public async Task<BlogPostViewModel> HandleAsync(GetBlogPostBySlugViewModelQuery query)
    {
      var post = await _context.BlogPosts
                          .AsNoTracking()
                          .Include(bp => bp.Author)
                          .Include(bp => bp.Comments)
                          .Include(bp => bp.BlogPostCategory)
                            .ThenInclude(bpc => bpc.Category)
                          .Where(bp => bp.Slug == query.Slug && bp.Public == true && bp.PublishOn < DateTime.Now)
                          .FirstOrDefaultAsync();

      if (post == null)
        return null;

      var bpvm = new BlogPostViewModel()
      {
        Id = post.Id,
        Slug = post.Slug,
        Author = post.Author,
        Title = post.Title,
        Description = post.Description,
        Content = post.Content,
        Comments = post.Comments,
        ModifiedAt = post.ModifiedAt,
        Categories = post.BlogPostCategory.Select(c => c.Category).ToList()
      };

      return bpvm;
    }
  }
}

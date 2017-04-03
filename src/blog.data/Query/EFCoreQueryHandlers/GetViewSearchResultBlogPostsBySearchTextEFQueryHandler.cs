using System;
using System.Collections.Generic;
using System.Text;
using blog.Models.ViewModels.Home;
using blog.Models;
using blog.EFCore;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using blog.data.Query.Queries;

namespace blog.data.Query.EFCoreQueryHandlers
{
  public class GetViewSearchResultBlogPostsBySearchTextEFQueryHandler : IQueryHandlerAsync<GetViewSearchResultBlogPostsBySearchText, IList<ViewSearchResultBlogPostViewModel>>
  {
    BlogDbContext _context;

    public GetViewSearchResultBlogPostsBySearchTextEFQueryHandler(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<IList<ViewSearchResultBlogPostViewModel>> HandleAsync(GetViewSearchResultBlogPostsBySearchText query)
    {
      var posts = await _context.BlogPosts
                          .AsNoTracking()
                          .FromSql("SELECT * from [dbo].[BlogPost] WHERE Contains((Content, Description, Title), {0})", query.searchText)
                          .Where(bp => bp.Public == true && bp.PublishOn < DateTime.Now)
                          .OrderByDescending(bp => bp.ModifiedAt)
                          .ThenByDescending(bp => bp.PublishOn)
                          .Include(bp => bp.Author)
                          .Include(bp => bp.BlogPostCategory)
                            .ThenInclude(bpc => bpc.Category)
                          .ToListAsync();

      var bps = new List<ViewSearchResultBlogPostViewModel>();

      foreach (var post in posts)
      {

        var categories = post.BlogPostCategory.Select(c => c.Category).ToList();

        var vsrbpvm = new ViewSearchResultBlogPostViewModel()
        {
          BlogPost = post,
          Categories = categories
        };

        bps.Add(vsrbpvm);

      }

      return bps;
    }
  }
}

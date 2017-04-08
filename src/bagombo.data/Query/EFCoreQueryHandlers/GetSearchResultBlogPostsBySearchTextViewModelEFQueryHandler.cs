using System;
using System.Collections.Generic;
using System.Text;
using Bagombo.Models.ViewModels.Home;
using Bagombo.Models;
using Bagombo.EFCore;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Bagombo.Data.Query.Queries;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public class GetSearchResultBlogPostsBySearchTextViewModelEFQueryHandler : IQueryHandlerAsync<GetSearchResultBlogPostsBySearchTextViewModel, IList<SearchResultBlogPostViewModel>>
  {
    BlogDbContext _context;

    public GetSearchResultBlogPostsBySearchTextViewModelEFQueryHandler(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<IList<SearchResultBlogPostViewModel>> HandleAsync(GetSearchResultBlogPostsBySearchTextViewModel query)
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

      var bps = new List<SearchResultBlogPostViewModel>();

      foreach (var post in posts)
      {

        var categories = post.BlogPostCategory.Select(c => c.Category).ToList();

        var vsrbpvm = new SearchResultBlogPostViewModel()
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

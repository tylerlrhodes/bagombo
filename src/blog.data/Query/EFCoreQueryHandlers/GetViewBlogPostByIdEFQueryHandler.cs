﻿using blog.data.Query.Queries;
using blog.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using blog.EFCore;

namespace blog.data.Query.EFCoreQueryHandlers
{
  public class GetViewBlogPostByIdEFQueryHandler : IQueryHandlerAsync<GetViewBlogPostById, ViewBlogPostViewModel>
  {
    BlogDbContext _context;

    public GetViewBlogPostByIdEFQueryHandler(BlogDbContext context)
    {
      _context = context;
    }

    public async Task<ViewBlogPostViewModel> HandleAsync(GetViewBlogPostById query)
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

      var bpvm = new ViewBlogPostViewModel()
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
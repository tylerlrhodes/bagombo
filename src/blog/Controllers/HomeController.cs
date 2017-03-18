using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using blog.Data;
using blog.Models.ViewModels.Home;
using blog.Models;
using CommonMark;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace blog.Controllers
{
  public class HomeController : Controller
  {
    BlogDbContext _context;

    public HomeController(BlogDbContext context)
    {
      _context = context;
    }

    public IActionResult Index()
    {
      ViewData["Title"] = "Bagombo Home";
      return View();
    }

    public IActionResult RecentPosts()
    {
      return View();
    }

    public IActionResult AllPosts()
    {
      return View();
    }

    public async Task<IActionResult> FeaturePosts(int id)
    {
      var feature = await _context.Features.FindAsync(id);

      var bps = await _context.BlogPostFeature
                              .Where(bpf => bpf.FeatureId == feature.Id && bpf.BlogPost.Public == true && bpf.BlogPost.PublishOn < DateTime.Now)
                              .Select(bpf => bpf.BlogPost)
                              .ToListAsync();

      var posts = await _context.BlogPosts
                  .Include(bp => bp.Author)
                  .Include(bp => bp.Categories)
                  .Where(bp => bps.Contains(bp))
                  .ToListAsync();

      List<ViewBlogPostViewModel> viewPosts = new List<ViewBlogPostViewModel>();

      foreach (var p in posts)
      {
        //var categories = p.Categories.Select(c => c.Category).ToList();

        var categoryIds = p.Categories.Select(c => c.CategoryId);

        var categories = await (from cat in _context.Categories
                                where categoryIds.Contains(cat.Id)
                                select cat).ToListAsync();

        var bpView = new ViewBlogPostViewModel()
        {
          Author = $"{p.Author.FirstName} {p.Author.LastName}",
          Title = p.Title,
          Description = p.Description,
          Categories = categories,
          ModifiedAt = p.ModifiedAt,
          Id = p.Id
        };
        viewPosts.Add(bpView);
      }

      ViewFeaturePostsViewModel vfpvm = new ViewFeaturePostsViewModel()
      {
        Feature = feature,
        BlogPosts = viewPosts
      };

      return View(vfpvm);
    }

    public async Task<IActionResult> Features()
    {
      // bug in EF Core that needs a bit more code here than otherwise
      // see: https://github.com/aspnet/EntityFramework/issues/7714

      ViewFeaturesViewModel vfvm = new ViewFeaturesViewModel();

      // cant select new into a defined type so have to use anon type for the select new here due to EF Core bug
      // code after is a work-around

      var x = from feature in _context.Features
              select new
              {
                Title = feature.Title,
                Description = feature.Description,
                Id = feature.Id,
                BlogCount = (from posts in _context.BlogPostFeature
                             where posts.FeatureId == feature.Id && posts.BlogPost.Public == true && posts.BlogPost.PublishOn < DateTime.Now
                             select posts).Count()
              };

      List<FeatureViewModel> featureList = new List<FeatureViewModel>();

      foreach (var feature in await x.OrderByDescending(f => f.BlogCount).ToListAsync())
      {
        FeatureViewModel fvm = new FeatureViewModel()
        {
          BlogCount = feature.BlogCount,
          Title = feature.Title,
          Description = feature.Description,
          Id = feature.Id
        };
        featureList.Add(fvm);
      }

      vfvm.Features = featureList;

      return View(vfvm);
    }

    public IActionResult About()
    {
      return View();
    }

    public async Task<IActionResult> BlogPost(int? id)
    {
      if (id == null)
        return NotFound();

      var post = await _context.BlogPosts
                          .Include(bp => bp.Author)
                          .Include(bp => bp.Categories)
                          .Where(bp => bp.Id == id && bp.Public == true && bp.PublishOn < DateTime.Now)
                          .FirstOrDefaultAsync();

      if (post == null)
        return NotFound();

      var categoryIds = post.Categories.Select(c => c.CategoryId);

      var categories = await (from cat in _context.Categories
                              where categoryIds.Contains(cat.Id)
                              select cat).ToListAsync();

      var bpvm = new ViewBlogPostViewModel()
      {
        Author = $"{post.Author.FirstName} {post.Author.LastName}",
        Title = post.Title,
        Description = post.Description,
        Content = post.Content,
        ModifiedAt = post.ModifiedAt,
        Categories = categories
      };

      bpvm.Content = CommonMarkConverter.Convert(bpvm.Content);

      return View(bpvm);
    }

  }

}

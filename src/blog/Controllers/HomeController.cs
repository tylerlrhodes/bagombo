using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using blog.Data;
using blog.Models.ViewModels.Home;
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

      var bp = await (from bps in _context.BlogPosts
                      where bps.Id == id && bps.Public == true
                      join a in _context.Authors on bps.AuthorId equals a.Id
                      select new ViewBlogPostViewModel
                      {
                        Author = $"{a.FirstName} {a.LastName}",
                        Title = bps.Title,
                        Description = bps.Description,
                        Content = bps.Content,
                        ModifiedAt = bps.ModifiedAt
                      })
                      .FirstOrDefaultAsync();

      if (bp == null)
        return NotFound();

      bp.Content = CommonMarkConverter.Convert(bp.Content);

      return View(bp);
    }

  }

}

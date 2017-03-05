using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using blog.Infrastructure;
using blog.Data;
using blog.Models.ViewModels.Home;
//using blog.Models.ViewModels.BlogPostView;
using CommonMark;

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
    // GET: /<controller>/
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
    public IActionResult About()
    {
      return View();
    }
    public IActionResult BlogPost(int? id)
    {
      if (id == null)
        return NotFound();

      var bp = (from bps in _context.BlogPosts
               where bps.Id == id
               join a in _context.Authors on bps.AuthorId equals a.Id
               select new ViewBlogPostViewModel
               {
                 Author = $"{a.FirstName} {a.LastName}",
                 Title = bps.Title,
                 Description = bps.Description,
                 Content = bps.Content,
                 ModifiedAt = bps.ModifiedAt
               })
               .FirstOrDefault();

      if (bp == null)
        return NotFound();

      return View(bp);
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using blog.Infrastructure;
using blog.Data;
using blog.Models;
using blog.Models.ViewModels.BlogPostView;
using CommonMark;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace blog.Controllers
{
  public class HomeController : Controller
  {
    // GET: /<controller>/
    public IActionResult Index()
    {
      ViewBag.Title = "Bagombo Home";
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
    public IActionResult BlogPost(int? id, [FromServices] BlogContext _context)
    {
      if (id != null)
      {
        var x = from bp in _context.BlogPosts
                where bp.Id == id
                join a in _context.Authors on bp.AuthorId equals a.Id
                select new BlogPostViewModel
                {
                  Author = $"{a.FirstName} {a.LastName}",
                  Content = bp.Content,
                  Title = bp.Title
                };

        BlogPostViewModel bpvm = x.FirstOrDefault();

        bpvm.Content = CommonMark.CommonMarkConverter.Convert(bpvm.Content);

        if (bpvm != null)
          return View(bpvm);
      }

      return NotFound();
    }
  }
}

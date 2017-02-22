using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using blog.Infrastructure;
using blog.Data;
using blog.Models;
using Microsoft.EntityFrameworkCore;
using blog.ViewModels.Admin;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace blog.Controllers
{
  public class AdminController : Controller
  {
    BlogContext _context;
    public AdminController(BlogContext context)
    {
      _context = context;
    }
    // GET: /<controller>/
    public IActionResult Index()
    {
      return View();
    }
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpGet]
    public IActionResult ListPosts()
    {
      ListPostsViewModel lpvm = new ListPostsViewModel();
      lpvm.posts = _context.BlogPosts.Include(a => a.Author).AsEnumerable();
      return View(lpvm);
    }
    [HttpGet]
    public IActionResult AddPost()
    {
      return View();
    }
    [HttpGet]
    public IActionResult EditPost(int id)
    {
      BlogPost post = _context.BlogPosts.Where(p => p.Id == id).Include(a => a.Author).FirstOrDefault();
      if (post != null)
      {
        return View(post);
      }
      return RedirectToAction("ListPosts", "Admin");
    }
  }

}

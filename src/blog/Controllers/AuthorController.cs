using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using blog.Models;
using blog.Data;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace blog.Controllers
{
  [Authorize(Roles = "Authors")]
  public class AuthorController : Controller
  {
    BlogDbContext _context;

    public AuthorController(BlogDbContext context)
    {
      _context = context;
    }
    // GET: /<controller>/
    public IActionResult Index()
    {
      return View();
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

    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpGet]
    public IActionResult ManagePosts()
    {
      return NotFound();
    }
  }

}

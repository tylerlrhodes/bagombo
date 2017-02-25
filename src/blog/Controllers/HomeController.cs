using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using blog.Infrastructure;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace blog.Controllers
{
  public class HomeController : Controller
  {
    // GET: /<controller>/
    public IActionResult Index()
    {
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
    public IActionResult NoWhere()
    {
      return NotFound();
    }
  }
}

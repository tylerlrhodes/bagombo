using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using blog.Infrastructure;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace blog.Controllers
{
  public class AdminController : Controller
  {
    // GET: /<controller>/
    public IActionResult Index()
    {
      return View();
    }
    [HttpsOnly]
    public IActionResult AddPost()
    {
      return View();
    }
  }
}

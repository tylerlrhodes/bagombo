using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using blog.Models.ViewModels.Account;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace blog.Controllers
{
  public class AccountController : Controller
  {
    // GET: /<controller>/
    public IActionResult Index()
    {
      return View();
    }
    public IActionResult Login()
    {
      return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(AccountLoginViewModel alvm)
    {

      return NotFound();
    }
  }
}

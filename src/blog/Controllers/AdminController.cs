using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using blog.Infrastructure;
using blog.Data;
using blog.Models;
using Microsoft.EntityFrameworkCore;
using blog.Models.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace blog.Controllers
{
  [Authorize(Roles = "Admins")]
  public class AdminController : Controller
  {
    BlogContext _context;
    UserManager<ApplicationUser> _userManager;
    IPasswordHasher<ApplicationUser> _passwordHasher;
    IUserValidator<ApplicationUser> _userValidator;
    IPasswordValidator<ApplicationUser> _passwordValidator;

    public AdminController(BlogContext context,
                           UserManager<ApplicationUser> userManager,
                           IPasswordHasher<ApplicationUser> passwordHasher,
                           IPasswordValidator<ApplicationUser> passwordValidator,
                           IUserValidator<ApplicationUser> userValidator)
    {
      _context = context;
      _userManager = userManager;
      _passwordHasher = passwordHasher;
      _passwordValidator = passwordValidator;
      _userValidator = userValidator;
    }
    // GET: /<controller>/
    public IActionResult Index()
    {
      return View();
    }
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpGet]
    public IActionResult ManagePosts()
    {
      AdminManagePostsViewModel mpvm = new AdminManagePostsViewModel();
      mpvm.posts = _context.BlogPosts.Include(a => a.Author).AsEnumerable();
      return View(mpvm);
    }
    public IActionResult ManageUsers()
    {
      return View(_userManager.Users.Include(u => u.Logins));
    }
    public ViewResult CreateUser() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(UserViewModel model)
    {
      if (ModelState.IsValid)
      {
        ApplicationUser user = new ApplicationUser
        {
          UserName = model.Name,
          Email = model.Email
        };
        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
          return RedirectToAction("ManageUsers");
        }
        else
        {
          foreach (IdentityError error in result.Errors)
          {
            ModelState.AddModelError("", error.Description);
          }
        }
      }
      return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string id)
    {
      ApplicationUser user = await _userManager.FindByIdAsync(id);
      if(user != null)
      {
        IdentityResult result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
          return RedirectToAction("ManageUsers");
        }
        else
        {
          foreach (IdentityError error in result.Errors)
          {
            ModelState.AddModelError("", error.Description);
          }
        }
      }
      else
      {
        ModelState.AddModelError("", "User not found!");
      }
      return View("ManageUsers", _userManager.Users);
    }

    public async Task<IActionResult> EditUser(string id)
    {
      ApplicationUser user = await _userManager.FindByIdAsync(id);
      if (user != null)
      {
        return View(new UserViewModel {
          Id = user.Id,
          Name = user.UserName,
          Email = user.Email,
          Password = ""
        });
      }
      else
      {
        return RedirectToAction("ManageUsers");
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(UserViewModel user)
    {
      if (ModelState.IsValid)
      {
        ApplicationUser au = await _userManager.FindByIdAsync(user.Id);
        if (au != null)
        {
          au.Email = user.Email;
          au.UserName = user.Name;
          IdentityResult validUser = await _userValidator.ValidateAsync(_userManager, au);
          if (!validUser.Succeeded)
          {
            foreach (var error in validUser.Errors)
            {
              ModelState.AddModelError("", error.Description);
            }
          }
          var logins = await _userManager.GetLoginsAsync(au);
          if ( logins.Count() != 0)
          {
            IdentityResult result = await _userManager.UpdateAsync(au);
            if (result.Succeeded)
            {
              return RedirectToAction("ManageUsers");
            }
            else
            {
              foreach (var error in result.Errors)
              {
                ModelState.AddModelError("", error.Description);
              }
            }
            return RedirectToAction("ManageUsers");
          }
          if (!string.IsNullOrEmpty(user.Password))
          {
            IdentityResult validPassword = await _passwordValidator.ValidateAsync(_userManager, au, user.Password);
            if (validPassword.Succeeded)
            {
              au.PasswordHash = _passwordHasher.HashPassword(au, user.Password);
              IdentityResult securityStampUpdate = await _userManager.UpdateSecurityStampAsync(au);
              if (securityStampUpdate.Succeeded)
              { 
                IdentityResult result = await _userManager.UpdateAsync(au);
                if (result.Succeeded)
                {
                  return RedirectToAction("ManageUsers");
                }
                else
                {
                  foreach (var error in result.Errors)
                  {
                    ModelState.AddModelError("", error.Description);
                  }
                }
              }
              else
              {
                foreach (var error in securityStampUpdate.Errors)
                {
                  ModelState.AddModelError("", error.Description);
                }
              }
            }
            else
            {
              foreach (var error in validPassword.Errors)
              {
                ModelState.AddModelError("", error.Description);
              }
            }
          }
          else
          {
            ModelState.AddModelError("", "Password can't be empty");
          }
        }
      }
      return View(user);
    }
  }

}

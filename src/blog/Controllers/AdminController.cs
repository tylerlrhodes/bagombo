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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace blog.Controllers
{
  [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
  [Authorize(Roles = "Admins")]
  public class AdminController : Controller
  {
    BlogDbContext _context;
    UserManager<ApplicationUser> _userManager;
    //RoleManager<IdentityRole> _roleManager;
    IPasswordHasher<ApplicationUser> _passwordHasher;
    IUserValidator<ApplicationUser> _userValidator;
    IPasswordValidator<ApplicationUser> _passwordValidator;

    public AdminController(BlogDbContext context,
                           UserManager<ApplicationUser> userManager,
                           //RoleManager<ApplicationUser> roleManager,
                           IPasswordHasher<ApplicationUser> passwordHasher,
                           IPasswordValidator<ApplicationUser> passwordValidator,
                           IUserValidator<ApplicationUser> userValidator)
    {
      _context = context;
      _userManager = userManager;
      //_roleManager = roleManager;
      _passwordHasher = passwordHasher;
      _passwordValidator = passwordValidator;
      _userValidator = userValidator;
    }
    // GET: /<controller>/
    public IActionResult Index()
    {
      return View();
    }

    [HttpGet]
    public IActionResult ManagePosts()
    {
      AdminManagePostsViewModel mpvm = new AdminManagePostsViewModel();
      mpvm.posts = _context.BlogPosts.Include(a => a.Author).AsEnumerable();
      return View(mpvm);
    }
    public IActionResult ManageUsers()
    {
      return View(_userManager.Users.Include(u => u.Logins).Include(u => u.Author));
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
          UserName = model.UserName,
          Email = model.Email
        };
        if (model.Password == null)
        {
          ModelState.AddModelError("", "Password cannot be blank.");
          return View(model);
        }
        if (model.IsAuthor == true)
        {
          if (String.IsNullOrEmpty(model.FirstName) || String.IsNullOrEmpty(model.LastName))
          {
            ModelState.AddModelError("", "First and last name required for authors.");
            return View(model);
          }
          Author newAuthor = new Author
          {
            FirstName = model.FirstName,
            LastName = model.LastName
          };
          user.Author = newAuthor;
        }
        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        if (user.Author != null)
        {
          await _userManager.AddToRoleAsync(user, "Authors");
        }
        if (result.Succeeded && model.IsAuthor == false)
        {
          return RedirectToAction("ManageUsers");
        }
        if (result.Succeeded && model.IsAuthor == true)
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

    public IActionResult EditUser(string id)
    {
      ApplicationUser user = _userManager.Users.Where(u => u.Id == id).Include(u => u.Author).FirstOrDefault();

      if (user != null)
      {
        return View(new UserViewModel {
          Id = user.Id,
          UserName = user.UserName,
          Email = user.Email,
          Password = "",
          IsAuthor = user.Author == null ? false : true,
          FirstName = user.Author?.FirstName,
          LastName = user.Author?.LastName
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
        ApplicationUser au = _userManager.Users.Where(u => u.Id == user.Id).Include(u => u.Author).FirstOrDefault();

        if (user.IsAuthor == true)
        { 
          if (String.IsNullOrEmpty(user.FirstName) || String.IsNullOrEmpty(user.LastName))
          {
            ModelState.AddModelError("", "Author requires both first and last name to be set.");
            return View(user);
          }
          if (au.Author == null)
          {
            Author author = new Author
            {
              FirstName = user.FirstName,
              LastName = user.LastName
            };
            au.Author = author;
            await _userManager.AddToRoleAsync(au, "Authors");
          }
        }
        else
        {
          if (au.Author != null)
          {
            _context.Authors.Remove(au.Author);
            await _userManager.RemoveFromRoleAsync(au, "Authors");
          }
        }
        if (au != null)
        {
          au.Email = user.Email;
          au.UserName = user.UserName;
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

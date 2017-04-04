using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bagombo.EFCore;
using Bagombo.Models;
using Microsoft.EntityFrameworkCore;
using Bagombo.Models.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


namespace Bagombo.Controllers
{
  [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
  [Authorize(Roles = "Admins")]
  public class AdminController : Controller
  {
    BlogDbContext _context;
    UserManager<ApplicationUser> _userManager;
    SignInManager<ApplicationUser> _signInManager;
    IPasswordHasher<ApplicationUser> _passwordHasher;
    IUserValidator<ApplicationUser> _userValidator;
    IPasswordValidator<ApplicationUser> _passwordValidator;

    public AdminController(BlogDbContext context,
                           UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           IPasswordHasher<ApplicationUser> passwordHasher,
                           IPasswordValidator<ApplicationUser> passwordValidator,
                           IUserValidator<ApplicationUser> userValidator)
    {
      _context = context;
      _userManager = userManager;
      _signInManager = signInManager;
      _passwordHasher = passwordHasher;
      _passwordValidator = passwordValidator;
      _userValidator = userValidator;
    }

    public IActionResult Index()
    {
      return View();
    }

    [HttpGet]
    public IActionResult AddFeature() => View();

    [HttpPost]
    public async Task<IActionResult> AddFeature(FeatureViewModel model)
    {
      if (ModelState.IsValid)
      {
        var f = new Feature()
        {
          Title = model.Title,
          Description = model.Description
        };
        _context.Features.Add(f);
        await _context.SaveChangesAsync();
        return RedirectToAction("ManageFeatures");
      }
      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditFeature(long id)
    {
      var f = await _context.Features.FindAsync(id);
      var vm = new FeatureViewModel()
      {
        Id = f.Id,
        Title = f.Title,
        Description = f.Description
      };
      return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> EditFeature(FeatureViewModel model)
    {
      if (ModelState.IsValid)
      {
        var f = await _context.Features.FindAsync(model.Id);
        f.Title = model.Title;
        f.Description = model.Description;
        await _context.SaveChangesAsync();
        return RedirectToAction("ManageFeatures");
      }
      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ManageFeatures()
    {
      AdminManageFeaturesViewModel amfvm = new AdminManageFeaturesViewModel();
      amfvm.Features = await _context.Features.AsNoTracking().ToListAsync();
      return View(amfvm);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteFeature(long id)
    {
      var feature = await _context.Features.FindAsync(id);
      _context.Features.Remove(feature);
      await _context.SaveChangesAsync();
      return RedirectToAction("ManageFeatures");
    }
    
    [HttpGet]
    public IActionResult AddCategory() => View();

    [HttpPost]
    public async Task<IActionResult> AddCategory(CategoryViewModel model)
    {
      if (ModelState.IsValid)
      {
        var c = new Category()
        {
          Name = model.Name,
          Description = model.Description
        };
        _context.Categories.Add(c);
        await _context.SaveChangesAsync();
        return RedirectToAction("ManageCategories");
      }
      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditCategory(long id)
    {
      var c = await _context.Categories.FindAsync(id);
      var vm = new CategoryViewModel()
      {
        Name = c.Name,
        Description = c.Description,
        Id = c.Id
      };
      return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> EditCategory(CategoryViewModel model)
    {
      if (ModelState.IsValid)
      {
        var c = await _context.Categories.FindAsync(model.Id);
        c.Name = model.Name;
        c.Description = model.Description;
        await _context.SaveChangesAsync();
        return RedirectToAction("ManageCategories");
      }
      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ManageCategories()
    {
      AdminManageCategoriesViewModel amcvm = new AdminManageCategoriesViewModel();
      amcvm.Categories = await _context.Categories.AsNoTracking().ToListAsync();
      return View(amcvm);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCategory(long id)
    {
      var c = await _context.Categories.FindAsync(id);
      _context.Categories.Remove(c);
      await _context.SaveChangesAsync();
      return RedirectToAction("ManageCategories");
    }

    [HttpGet]
    public async Task<IActionResult> ManagePosts()
    {
      AdminManagePostsViewModel mpvm = new AdminManagePostsViewModel();
      mpvm.posts = await _context.BlogPosts.AsNoTracking().Include(a => a.Author).ToListAsync();
      return View(mpvm);
    }

    public async Task<IActionResult> ManageUsers()
    {
      return View(await _userManager.Users.AsNoTracking().Include(u => u.Logins).Include(u => u.Author).ToListAsync());
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
      if (user != null)
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
      ApplicationUser user = _userManager.Users.AsNoTracking().Where(u => u.Id == id).Include(u => u.Author).Include(u => u.Logins).FirstOrDefault();

      if (user != null)
      {
        return View(new UserViewModel
        {
          Id = user.Id,
          UserName = user.UserName,
          Email = user.Email,
          Password = "",
          IsAuthor = user.Author == null ? false : true,
          FirstName = user.Author?.FirstName,
          LastName = user.Author?.LastName,
          ExternalLogins = user.Logins.Count() == 0 ? false : true
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
        //Get some info on the user
        ApplicationUser au = _userManager.Users.Where(u => u.Id == user.Id).Include(u => u.Author).FirstOrDefault();
        // Make the user an author if it's not already, no author fields can be "updated right now"
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
            try
            {
              await _userManager.AddToRoleAsync(au, "Authors");
            }
            catch (Exception)
            {
              // how to handle non unique entry to Author ....
              ModelState.AddModelError("", "Error making the user an author, perhaps first and last name are not unique in database");
              return View(user);
            }
          }
        }
        // It's not an author
        else
        {
          if (au.Author != null)
          {
            // Remove it from authors if it was and remove the role
            _context.Authors.Remove(au.Author);
            await _userManager.RemoveFromRoleAsync(au, "Authors");
          }
        }
        // The user should not be null!
        if (au != null)
        {
          // Logic goes like this:
          // Validate user with new email and username
          // If it has an external login, just change the stuff and update the user assuming it passes validation
          au.Email = user.Email;
          au.UserName = user.UserName;
          IdentityResult validUser = await _userValidator.ValidateAsync(_userManager, au);
          if (!validUser.Succeeded)
          {
            foreach (var error in validUser.Errors)
            {
              ModelState.AddModelError("", error.Description);
              return View(user);
            }
          }
          var logins = await _userManager.GetLoginsAsync(au);
          if (logins.Count() != 0)
          {
            IdentityResult result = await _userManager.UpdateAsync(au);
            if (au == await _userManager.GetUserAsync(User))
            {
              await _signInManager.RefreshSignInAsync(au);
            }
            if (result.Succeeded)
            {
              return RedirectToAction("ManageUsers");
            }
            else
            {
              // something happened, throw an exception or something
              // for now just return to the edit page
            }
          }
          else
          {
            // If the password string isn't empty and they want to change the password
            if (!string.IsNullOrEmpty(user.Password) && user.ChangePassword == true)
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
                    if (au == await _userManager.GetUserAsync(User))
                    {
                      await _signInManager.RefreshSignInAsync(au);
                    }
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
            // Can't change the password and have no password there!
            else if (string.IsNullOrEmpty(user.Password) && user.ChangePassword == true)
            {
              ModelState.AddModelError("", "Password can't be empty");
            }
            // Just update the user and don't change the password
            else
            {
              IdentityResult result = await _userManager.UpdateAsync(au);
              if (result.Succeeded)
              {
                if (au == await _userManager.GetUserAsync(User))
                {
                  await _signInManager.RefreshSignInAsync(au);
                }
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
          }
          // end if au == null
        }
      }
      return View(user);
    }

  }

}

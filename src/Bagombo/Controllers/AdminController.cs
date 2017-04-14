using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bagombo.EFCore;
using Bagombo.Models;
using Bagombo.Data.Query;
using Bagombo.Data.Query.Queries;
using Bagombo.Data.Command.Commands;
using Bagombo.Data.Command;
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
    private ICommandProcessor _cp;
    private IQueryProcessorAsync _qpa;
    BlogDbContext _context;
    UserManager<ApplicationUser> _userManager;
    SignInManager<ApplicationUser> _signInManager;
    IPasswordHasher<ApplicationUser> _passwordHasher;
    IUserValidator<ApplicationUser> _userValidator;
    IPasswordValidator<ApplicationUser> _passwordValidator;

    public AdminController(ICommandProcessor cp,
                           IQueryProcessorAsync qpa,
                           BlogDbContext context,
                           UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           IPasswordHasher<ApplicationUser> passwordHasher,
                           IPasswordValidator<ApplicationUser> passwordValidator,
                           IUserValidator<ApplicationUser> userValidator)
    {
      _cp = cp;
      _qpa = qpa;
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
        var afc = new AddFeatureCommand()
        {
          Feature = new Feature()
          {
            Title = model.Title,
            Description = model.Description
          }
        };

        var result = await _cp.ProcessAsync(afc);

        if (result.Succeeded)
        {
          return RedirectToAction("ManageFeatures"); 
        }
        else
        {
          // To do, better exeception handling
          return NotFound();
        }
      }
      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditFeature(long id)
    {
      var gfbivm = new GetFeatureByIdQuery()
      {
        Id = id
      };

      var f = await _qpa.ProcessAsync(gfbivm);

      if (f != null)
      {
        return View(new FeatureViewModel() { Id = f.Id, Title = f.Title, Description = f.Description } );
      }
      else
        // need better exception and error handling
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> EditFeature(FeatureViewModel model)
    {
      if (ModelState.IsValid)
      {
        var efc = new EditFeatureCommand()
        {
          Id = model.Id,
          NewTitle = model.Title,
          NewDescription = model.Description
        };

        var result = await _cp.ProcessAsync(efc);

        if (result.Succeeded)
        {
          return RedirectToAction("ManageFeatures");
        }
        else
        {
          // To Do - Better Error handling
          return NotFound();
        }
      }
      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ManageFeatures()
    {
      var gmfvmq = new GetManageFeaturesViewModelQuery();

      var mfvm = await _qpa.ProcessAsync(gmfvmq);

      return View(mfvm);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteFeature(long id)
    {
      var dfc = new DeleteFeatureCommand()
      {
        Id = id
      };

      var result = await _cp.ProcessAsync(dfc);

      if (result.Succeeded)
      {
        return RedirectToAction("ManageFeatures"); 
      }
      else
      {
        return NotFound();
      }
    }
    
    [HttpGet]
    public IActionResult AddCategory() => View();

    [HttpPost]
    public async Task<IActionResult> AddCategory(CategoryViewModel model)
    {
      if (ModelState.IsValid)
      {
        var acc = new AddCategoryCommand()
        {
          Name = model.Name,
          Description = model.Description
        };

        var result = await _cp.ProcessAsync(acc);

        if (result.Succeeded)
        {
          return RedirectToAction("ManageCategories"); 
        }
        else
        {
          return NotFound();
        }
      }
      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditCategory(long id)
    {
      var gcbiq = new GetCategoryByIdQuery()
      {
        Id = id
      };

      var c = await _qpa.ProcessAsync(gcbiq);

      if (c != null)
      {
        return View(new CategoryViewModel() { Id = c.Id, Name = c.Name, Description = c.Description }); 
      }
      else
      {
        return NotFound();
      }
    }

    [HttpPost]
    public async Task<IActionResult> EditCategory(CategoryViewModel model)
    {
      if (ModelState.IsValid)
      {
        var ecc = new EditCategoryCommand()
        {
          Id = model.Id,
          NewName = model.Name,
          NewDescription = model.Description
        };

        var result = await _cp.ProcessAsync(ecc);

        if (result.Succeeded)
        {
          return RedirectToAction("ManageCategories"); 
        }
        else
        {
          return NotFound();
        }
      }
      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ManageCategories()
    {
      var gmcvmq = new GetManageCategoriesViewModelQuery();

      var amcvm = await _qpa.ProcessAsync(gmcvmq);

      return View(amcvm);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCategory(long id)
    {
      var dcc = new DeleteCategoryCommand()
      {
        Id = id
      };

      var result = await _cp.ProcessAsync(dcc);

      if (result.Succeeded)
      {
        return RedirectToAction("ManageCategories"); 
      }
      else
      {
        return NotFound();
      }
    }

    [HttpGet]
    public async Task<IActionResult> ManagePosts()
    {
      var gmpvmq = new GetManagePostsViewModelQuery();

      var mpvm = await _qpa.ProcessAsync(gmpvmq);

      return View(mpvm);
    }

    public async Task<IActionResult> ManageUsers()
    {
      // This was previously done all through EF Core, just including the author in the below query
      // However, in an effort to decouple the application classes from Framework classes
      // it has been seperated into two queries
      var users = await _userManager.Users.AsNoTracking().Include(u => u.Logins).ToListAsync();

      var authors = await _qpa.ProcessAsync(new GetAuthorsDictionaryKeyAppUserIdQuery());

      foreach(var user in users)
      {
        if (authors.ContainsKey(user.Id))
        {
          user.Author = authors[user.Id];
        }
      }

      return View(users);
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
          // create command to create author, don't use EF for Author related stuff in Controllers
          // In an effort to decouple application from Framework
          // user.Author = newAuthor;
          var aac = new AddAuthorCommand()
          {
            ApplicatoinUserId = user.Id,
            FirstName = model.FirstName,
            LastName = model.LastName
          };

          var aacResult = await _cp.ProcessAsync(aac);

          if (aacResult.Succeeded)
          {
            user.Author = aacResult.Command.Author;
          }
        }
        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        if (user.Author != null)
        {
          await _userManager.AddToRoleAsync(user, "Authors");
        }
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
      if (user != null)
      {
        // This set's the corresponding author to null if there is one
        // need to remove the EF Core mapping and handle the relationship manually

        // check if the user is an author

        if (await _qpa.ProcessAsync(new GetIsUserAnAuthorQuery { Id = user.Id }))
        {
          // set its App User Id field to null
          var commandResult = await _cp.ProcessAsync(new SetAppUserIdNullForAuthorCommand { Id = user.Id });

          if (!commandResult.Succeeded)
          {
            // need better error handling ....
            return NotFound();
          }
        }
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
      ApplicationUser user = _userManager.Users.AsNoTracking().Where(u => u.Id == id).Include(u => u.Logins).FirstOrDefault();

      if (user != null)
      {
        var author = await _qpa.ProcessAsync(new GetAuthorByAppUserIdQuery { Id = user.Id });

        if (author != null)
        {
          user.Author = author;
        }

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
    public async Task<IActionResult> EditUser(UserViewModel model)
    {
      if (ModelState.IsValid)
      {
        //Get some info on the user
        ApplicationUser au = _userManager.Users.Where(u => u.Id == model.Id).Include(u => u.Author).FirstOrDefault();
        // Make the user an author if it's not already, no author fields can be "updated right now"
        if (model.IsAuthor == true)
        {
          if (String.IsNullOrEmpty(model.FirstName) || String.IsNullOrEmpty(model.LastName))
          {
            ModelState.AddModelError("", "Author requires both first and last name to be set.");
            return View(model);
          }
          if (au.Author == null)
          {
            Author author = new Author
            {
              FirstName = model.FirstName,
              LastName = model.LastName
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
              return View(model);
            }
          }
          // Add in code so you can fix the author's name
          // To Do
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
          au.Email = model.Email;
          au.UserName = model.UserName;
          IdentityResult validUser = await _userValidator.ValidateAsync(_userManager, au);
          if (!validUser.Succeeded)
          {
            foreach (var error in validUser.Errors)
            {
              ModelState.AddModelError("", error.Description);
              return View(model);
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
            if (!string.IsNullOrEmpty(model.Password) && model.ChangePassword == true)
            {
              IdentityResult validPassword = await _passwordValidator.ValidateAsync(_userManager, au, model.Password);
              if (validPassword.Succeeded)
              {
                au.PasswordHash = _passwordHasher.HashPassword(au, model.Password);
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
            else if (string.IsNullOrEmpty(model.Password) && model.ChangePassword == true)
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
      return View(model);
    }

  }

}

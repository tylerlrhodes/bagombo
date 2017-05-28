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
using Microsoft.Extensions.Logging;

namespace Bagombo.Controllers
{
  [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
  [Authorize(Roles = "Admins")]
  public class AdminController : Controller
  {
    private ICommandProcessorAsync _cp;
    private IQueryProcessorAsync _qpa;
    private UserManager<ApplicationUser> _userManager;
    private SignInManager<ApplicationUser> _signInManager;
    private IPasswordHasher<ApplicationUser> _passwordHasher;
    private IUserValidator<ApplicationUser> _userValidator;
    private IPasswordValidator<ApplicationUser> _passwordValidator;
    private ILogger _logger;

    public AdminController(ICommandProcessorAsync cp,
                           IQueryProcessorAsync qpa,
                           UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           IPasswordHasher<ApplicationUser> passwordHasher,
                           IPasswordValidator<ApplicationUser> passwordValidator,
                           IUserValidator<ApplicationUser> userValidator,
                           ILogger<AdminController> logger)
    {
      _cp = cp;
      _qpa = qpa;
      _userManager = userManager;
      _signInManager = signInManager;
      _passwordHasher = passwordHasher;
      _passwordValidator = passwordValidator;
      _userValidator = userValidator;
      _logger = logger;
    }

    public IActionResult Index()
    {
      return View();
    }

    [HttpGet]
    public IActionResult AddTopic() => View();

    [HttpPost]
    public async Task<IActionResult> AddTopic(TopicViewModel model)
    {
      if (ModelState.IsValid)
      {
        var afc = new AddTopicCommand()
        {
          Topic = new Topic()
          {
            Title = model.Title,
            Description = model.Description
          }
        };

        var result = await _cp.ProcessAsync(afc);

        if (result.Succeeded)
        {
          _logger.LogInformation("Added Topic {0}.", model.Title);

          return RedirectToAction("ManageTopics"); 
        }
        else
        {
          _logger.LogWarning("Unable to add Topic {0}.", model.Title);

          // todo: better error handling
          ModelState.AddModelError("Error", "Unable to add topic, perhaps the title is not unique.");
        }
      }
      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditTopic(long id)
    {
      var gfbivm = new GetTopicByIdQuery()
      {
        Id = id
      };

      var f = await _qpa.ProcessAsync(gfbivm);

      if (f != null)
      {
        return View(new TopicViewModel() { Id = f.Id, Title = f.Title, Description = f.Description });
      }
      else
      {
        _logger.LogWarning("EditTopic called with non-existant Id {0}", id);
        // need better exception and error handling
        return NotFound();
      }
    }

    [HttpPost]
    public async Task<IActionResult> EditTopic(TopicViewModel model)
    {
      if (ModelState.IsValid)
      {
        var efc = new EditTopicCommand()
        {
          Id = model.Id,
          NewTitle = model.Title,
          NewDescription = model.Description
        };

        var result = await _cp.ProcessAsync(efc);

        if (result.Succeeded)
        {
          _logger.LogInformation("Updated topic with id {0} with title {1} and description {2}", model.Id, model.Title, model.Description);
          return RedirectToAction("ManageTopics");
        }
        else
        {
          _logger.LogWarning("Unable to update topic with id {0} to title {1} and description {2}", model.Id, model.Title, model.Description);
          // To Do - Better Error handling
          ModelState.AddModelError("Error", "Unable to update topic, perhaps the title is not unique.");
        }
      }
      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ManageTopics()
    {
      var gmfvmq = new GetManageTopicsViewModelQuery();

      var mfvm = await _qpa.ProcessAsync(gmfvmq);

      return View(mfvm);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTopic(long id)
    {
      var dfc = new DeleteTopicCommand()
      {
        Id = id
      };

      var result = await _cp.ProcessAsync(dfc);

      if (result.Succeeded)
      {
        _logger.LogInformation("Deleted topic with id {0}", id);

        return RedirectToAction("ManageTopics"); 
      }
      else
      {
        _logger.LogWarning("Unable to delete topic with id {0}", id);
        // todo: better error handling
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
          _logger.LogInformation("Successfully added Category with name {0} and description {1}", model.Name, model.Description);

          return RedirectToAction("ManageCategories"); 
        }
        else
        {
          _logger.LogWarning("Unable to add category with name {0} and description {1}", model.Name, model.Description);
          // todo: better error handling
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
        _logger.LogWarning("EditCategory called with non-existant id {0}", id);
        // todo: better error handling
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
          _logger.LogInformation("Successfully updated Category with id {0} to name {1} and description {2}", model.Id, model.Name, model.Description);

          return RedirectToAction("ManageCategories"); 
        }
        else
        {
          _logger.LogWarning("Unable to update Category with id {0} to name {1} and description {2}", model.Id, model.Name, model.Description);
          // todo: better error handling
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
        _logger.LogInformation("Successfully deleted Category with id {0}", id);

        return RedirectToAction("ManageCategories"); 
      }
      else
      {
        _logger.LogWarning("Unable to delete category with id {0}", id);
        // todo: better error handling
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

    [HttpPost("{id}")]
    public async Task<IActionResult> DeletePost(long id)
    {
      var deletePostResult =
        await _cp.ProcessAsync(new DeleteBlogPostCommand
        {
          Id = id
        });

      if (deletePostResult.Succeeded)
      {
        _logger.LogInformation("Successfully deleted BlogPost with id {0}", id);

        return RedirectToAction("ManagePosts");
      }
      else
      {
        _logger.LogWarning("Unable to delete post with id {0}", id);
        // todo: better error handling
        return NotFound();
      }
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

        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        _logger.LogInformation("Creating user {0}", model.Email);

        if (model.IsAuthor == true && result.Succeeded)
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

            _logger.LogInformation("Setup user {0} as author {1} {2}", model.Email, model.FirstName, model.LastName);
          }
          else
          {
            _logger.LogWarning("Unable to setup user {0} as authoer {1} {2}", model.Email, model.FirstName, model.LastName);
          }
        }

        if (user.Author != null)
        {
          _logger.LogInformation("Adding Authors role to user {0}", model.Email);

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
            _logger.LogWarning("Unable to set AppUserId to Null for author with user {0} with id {1}", user.Email, user.Id);
            // need better error handling ....
            return NotFound();
          }
          else
          {
            _logger.LogInformation("Set AppUserId to Null for author with user {0} and id {1}", user.Email, user.Id);
          }
        }
        IdentityResult result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
          _logger.LogInformation("Removed user {0} with id {1}", user.Email, user.Id);

          return RedirectToAction("ManageUsers");
        }
        else
        {
          _logger.LogWarning("Unable to remove user {0} with id {1}", user.Email, user.Id);

          foreach (IdentityError error in result.Errors)
          {
            ModelState.AddModelError("", error.Description);
          }
        }
      }
      else
      {
        _logger.LogWarning("User not found with id {0}", id);

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
        _logger.LogWarning("EditUser called for non-existant user id {0}", id);

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
        ApplicationUser au = await _userManager.Users.Where(u => u.Id == model.Id).FirstOrDefaultAsync();

        // The user should not be null!
        if (au != null)
        {
          var author = await _qpa.ProcessAsync(new GetAuthorByAppUserIdQuery { Id = au.Id });

          if (author != null)
          {
            au.Author = author;
          }

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
              var aac = new AddAuthorCommand
              {
                ApplicatoinUserId = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName
              };

              var aacResult = await _cp.ProcessAsync(aac);

              if (aacResult.Succeeded)
              {
                _logger.LogInformation("Set user {0} to be author {1} {2}", au.Email, model.FirstName, model.LastName);

                await _userManager.AddToRoleAsync(au, "Authors");

                _logger.LogInformation("Added role Authors to user {0}", au.Email);

                au.Author = aacResult.Command.Author;
              }
              else
              {
                _logger.LogWarning("Unable to make user {0} author {1} {2}", au.Email, model.FirstName, model.LastName);

                ModelState.AddModelError("", "Error making the user an author, perhaps first and last name are not unique in database");
                return View(model);
              }
            }
            else
            {
              var uac = new UpdateAuthorCommand
              {
                Id = au.Author.Id,
                NewFirstName = model.FirstName,
                NewLastName = model.LastName
              };

              var uacResult = await _cp.ProcessAsync(uac);

              if (uacResult.Succeeded)
              {
                _logger.LogInformation("Updated author {0} {1} to {2} {3}", au.Author.FirstName, au.Author.LastName, model.FirstName, model.LastName);
                // do nothing
              }
              else
              {
                _logger.LogWarning("Unable to update author {0} {1} to {2} {3}", au.Author.FirstName, au.Author.LastName, model.FirstName, model.LastName);

                ModelState.AddModelError("", "Error updating the author, perhaps the first and last name are not unique in the database");
                return View(model);
              }
            }
          }
          // It's not an author
          else
          {
            if (au.Author != null)
            {
              // Remove it from authors if it was and remove the role

              var sauidnfa = new SetAppUserIdNullForAuthorCommand
              {
                Id = au.Id
              };

              var sauidnfaResult = await _cp.ProcessAsync(sauidnfa);

              if (sauidnfaResult.Succeeded)
              {
                _logger.LogInformation("Set AppUserId Null for Author {0} {1} with id {3}", au.Author.FirstName, au.Author.LastName, au.Author.Id);

                await _userManager.RemoveFromRoleAsync(au, "Authors");
              }
              else
              {
                // Error
                _logger.LogWarning("Unable to set AppUserId {0} to Null for Author {1} ", au.Id, au.Author.Id);

                ModelState.AddModelError("", "Error removing author from user!");
                return View(model);
              }
            }
          }
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
              _logger.LogInformation("Updated user {0} to new email {1} username {2}", au.Id, model.Email, model.UserName);

              return RedirectToAction("ManageUsers");
            }
            else
            {
              _logger.LogWarning("Unable to update user {0} to new email {1} username {2}", au.Id, model.Email, model.UserName);

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
                  _logger.LogInformation("Updated security stamp for user {0}", au.Id);

                  IdentityResult result = await _userManager.UpdateAsync(au);

                  if (result.Succeeded)
                  {
                    _logger.LogInformation("Updated user and password for user {0}", au.Id);

                    if (au == await _userManager.GetUserAsync(User))
                    {
                      await _signInManager.RefreshSignInAsync(au);
                    }
                    return RedirectToAction("ManageUsers");
                  }
                  else
                  {
                    _logger.LogWarning("Unable to update user and password for user {0}", au.Id);

                    foreach (var error in result.Errors)
                    {
                      ModelState.AddModelError("", error.Description);
                    }
                  }

                }
                else
                {
                  _logger.LogWarning("Unexpected error updating security stamp for user {0}", au.Id);

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
                _logger.LogInformation("Updated user {0}", au.Id);

                if (au == await _userManager.GetUserAsync(User))
                {
                  await _signInManager.RefreshSignInAsync(au);
                }
                return RedirectToAction("ManageUsers");
              }
              else
              {
                _logger.LogWarning("Unable to update user {0}", au.Id);

                foreach (var error in result.Errors)
                {
                  ModelState.AddModelError("", error.Description);
                }
              }

            }
          }
          // end if au == null
        }
        else
        {
          _logger.LogWarning("Edit user called with non-existant id {0}", model.Id);
        }
      }
      return View(model);
    }

  }

}

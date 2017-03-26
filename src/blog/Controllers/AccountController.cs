using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using blog.Models.ViewModels.Account;
using blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Authentication;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace blog.Controllers
{
  public class AccountController : Controller
  {
    UserManager<ApplicationUser> _userManager;
    SignInManager<ApplicationUser> _signInManager;
    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
      _userManager = userManager;
      _signInManager = signInManager;
    }
    // GET: /<controller>/
    public IActionResult Index()
    {
      return View();
    }
    public IActionResult Login(string returnUrl)
    {
      ViewData["returnUrl"] = returnUrl;
      return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(AccountLoginViewModel alvm, string returnUrl)
    {
      if (ModelState.IsValid)
      {
        ApplicationUser au = await _userManager.FindByEmailAsync(alvm.Email);
        if (au != null)
        {
          await _signInManager.SignOutAsync();
          Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(au, alvm.Password, false, false);
          if (result.Succeeded)
          {
            return Redirect(returnUrl ?? "/");
          }
        }
        ModelState.AddModelError(nameof(AccountLoginViewModel.Email), "Invalid user or passsword");
      }
      return View(alvm);
    }

    public async Task<IActionResult> Logoff()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Login");
    }

    public IActionResult FacebookLogin(string returnUrl)
    {
      string redirectUrl = Url.Action("FacebookResponse", "Account", new { ReturnUrl = returnUrl });
      AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
      return new ChallengeResult("Facebook", properties);
    }
    public async Task<IActionResult> FacebookResponse(string returnUrl = "/")
    {
      ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
      if (info == null)
      {
        return RedirectToAction(nameof(Login));
      }
      var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
      if (result.Succeeded)
      {
        return Redirect(returnUrl);
      }
      else
      {
        ViewData["ReturnUrl"] = returnUrl;
        ViewData["LoginProvider"] = info.LoginProvider;
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email });
      }
    }

    public IActionResult TwitterLogin(string returnUrl)
    {
      string redirectUrl = Url.Action("TwitterResponse", "Account", new { ReturnUrl = returnUrl });
      AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties("Twitter", redirectUrl);
      return new ChallengeResult("Twitter", properties);
    }

    public async Task<IActionResult> TwitterResponse(string returnUrl = "/")
    {
      ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
      if (info == null)
      {
        return RedirectToAction(nameof(Login));
      }
      var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
      if (result.Succeeded)
      {
        return Redirect(returnUrl);
      }
      else
      {
        ViewData["ReturnUrl"] = returnUrl;
        ViewData["LoginProvider"] = info.LoginProvider;
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email });
      }
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
    {
      // Get the information about the user from the external login provider
      var info = await _signInManager.GetExternalLoginInfoAsync();

      if (ModelState.IsValid)
      {
        if (info == null)
        {
          return View("ExternalLoginFailure");
        }
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
        {
          result = await _userManager.AddLoginAsync(user, info);
          if (result.Succeeded)
          {
            await _signInManager.SignInAsync(user, isPersistent: false);
            //_logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);
            return RedirectToLocal(returnUrl);
          }
        }
        else
        {
          ModelState.AddModelError("", result.ToString());
        }
        //AddErrors(result);
      }

      ViewData["ReturnUrl"] = returnUrl;
      ViewData["LoginProvider"] = info.LoginProvider;

      return View(model);
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
      if (Url.IsLocalUrl(returnUrl))
      {
        return Redirect(returnUrl);
      }
      else
      {
        return RedirectToAction(nameof(HomeController.Index), "Home");
      }
    }
    
  }
}

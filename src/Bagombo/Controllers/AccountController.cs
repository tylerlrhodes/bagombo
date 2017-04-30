using System.Security.Claims;
using System.Threading.Tasks;
using Bagombo.Models;
using Bagombo.Models.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bagombo.Controllers
{

  public class AccountController : Controller
  {
    private UserManager<ApplicationUser> _userManager;
    private SignInManager<ApplicationUser> _signInManager;

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
          Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(au, alvm.Password, alvm.RememberMe, false);
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

    [HttpPost]
    public IActionResult FacebookLogin(AccountLoginViewModel alvm, string returnUrl)
    {
      string redirectUrl = Url.Action("FacebookResponse", "Account", new { ReturnUrl = returnUrl, RememberMe = alvm.RememberMe });
      AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
      return new ChallengeResult("Facebook", properties);
    }

    public async Task<IActionResult> FacebookResponse(bool RememberMe, string returnUrl = "/")
    {
      ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
      if (info == null)
      {
        return RedirectToAction(nameof(Login));
      }

      var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, RememberMe);
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
    public IActionResult TwitterLogin(AccountLoginViewModel alvm, string returnUrl)
    {
      string redirectUrl = Url.Action("TwitterResponse", "Account", new { ReturnUrl = returnUrl, RememberMe = alvm.RememberMe });
      AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties("Twitter", redirectUrl);
      return new ChallengeResult("Twitter", properties);
    }

    public async Task<IActionResult> TwitterResponse(bool RememberMe, string returnUrl = "/")
    {
      ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
      if (info == null)
      {
        return RedirectToAction(nameof(Login));
      }

      var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, RememberMe);
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
            // _logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);
            return RedirectToLocal(returnUrl);
          }

        }
        else
        {
          ModelState.AddModelError("", result.ToString());
        }

        // AddErrors(result);
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

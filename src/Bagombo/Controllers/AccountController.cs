using System.Security.Claims;
using System.Threading.Tasks;
using Bagombo.Models;
using Bagombo.Models.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bagombo.Controllers
{

  public class AccountController : Controller
  {
    private UserManager<ApplicationUser> _userManager;
    private SignInManager<ApplicationUser> _signInManager;
    private ILogger _logger;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _logger = logger;
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
            _logger.LogInformation("Info - Successful login for {0}", au.Email);

            return Redirect(returnUrl ?? "/");
          }
        }

        _logger.LogWarning("Warning - unsuccessful login attempt for {0}", au.Email);

        ModelState.AddModelError(nameof(AccountLoginViewModel.Email), "Invalid user or passsword");
      }

      return View(alvm);
    }

    public async Task<IActionResult> Logoff()
    {
      var curUser = await _userManager.GetUserAsync(HttpContext.User);

      _logger.LogInformation("Info - Logout for {0}", curUser.Email);

      await _signInManager.SignOutAsync();

      return RedirectToAction("Login");
    }

    [HttpPost]
    public IActionResult FacebookLogin(AccountLoginViewModel alvm, string returnUrl)
    {
      string redirectUrl = Url.Action("FacebookResponse", "Account", new { ReturnUrl = returnUrl, RememberMe = alvm.RememberMe });
      var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
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
        _logger.LogInformation("Info - Facebook Login succeeded for {0}", info.Principal.FindFirstValue(ClaimTypes.Email));

        return Redirect(returnUrl);
      }
      else
      {
        _logger.LogInformation("Info - New registration started via Facebook for {0}", info.Principal.FindFirstValue(ClaimTypes.Email));

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
      var properties = _signInManager.ConfigureExternalAuthenticationProperties("Twitter", redirectUrl);
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
        _logger.LogInformation("Info - Twitter Login succeeded for {0}", info.Principal.FindFirstValue(ClaimTypes.Name));

        return Redirect(returnUrl);
      }
      else
      {
        _logger.LogInformation("Info - New registration started via Twitter for {0}", info.Principal.FindFirstValue(ClaimTypes.Name));

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

            _logger.LogInformation(6, "User created an account {0} using {1} provider.", model.Email, info.LoginProvider);

            return RedirectToLocal(returnUrl);
          }

        }
        else
        {
          _logger.LogWarning("Unable to create user with email {0} and login provider {1}.", model.Email, info.LoginProvider);

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

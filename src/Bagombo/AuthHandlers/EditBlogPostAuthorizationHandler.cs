using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Bagombo.Models;
using Bagombo.Data.Query;
using Microsoft.AspNetCore.Identity;
using Bagombo.Data.Query.Queries;

namespace Bagombo.AuthHandlers
{
  public class EditBlogPostAuthorizationHandler : AuthorizationHandler<SameAuthorRequirement, BlogPost>
  {
    private IQueryProcessorAsync _qpa;
    private UserManager<ApplicationUser> _userManager;

    public EditBlogPostAuthorizationHandler(IQueryProcessorAsync qpa,
                                            UserManager<ApplicationUser> userManager)
    {
      _userManager = userManager;
      _qpa = qpa;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SameAuthorRequirement requirement, BlogPost resource)
    {
      var user = await _userManager.GetUserAsync(context.User);

      var blogPostUserId = await _qpa.ProcessAsync(
        new GetUserIdFromBlogPost
        {
          blogpost = resource
        }
      );

      if (!string.IsNullOrEmpty(blogPostUserId) &&
          blogPostUserId == user.Id)
      {
        context.Succeed(requirement);
      }

    }
  }

  public class SameAuthorRequirement : IAuthorizationRequirement { }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace blog.Infrastructure
{
  public class HttpsOnlyAttribute : Attribute, IAuthorizationFilter
  {
    public void OnAuthorization(AuthorizationFilterContext context)
    {
      if (!context.HttpContext.Request.IsHttps)
      {
        context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
      }
    }
  }
}

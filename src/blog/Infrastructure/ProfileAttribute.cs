using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Text;

namespace blog.Infrastructure
{
  public class ProfileAttribute : ActionFilterAttribute
  {
    public override async Task OnActionExecutionAsync(ActionExecutingContext context,
                                                      ActionExecutionDelegate next)
    {
      Stopwatch timer = Stopwatch.StartNew();

      await next();

      timer.Stop();

      string result = "<div>Elapsed Time: " + $"{timer.Elapsed.TotalMilliseconds} ms</div>";
      byte[] bytes = Encoding.ASCII.GetBytes(result);

      await context.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
    } 
  }
}

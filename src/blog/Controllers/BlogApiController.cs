using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using blog.Models;
using blog.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace blog.Controllers
{
  [Authorize(Roles = "Admins")]
  [Route("api/[controller]/[action]")]
  public class BlogApiController : Controller
  {
    BlogDbContext _context;
    private readonly ILogger _logger;
    public BlogApiController(BlogDbContext context, ILogger<BlogApiController> logger)
    {
      _context = context;
      _logger = logger;
    }
    [HttpPost("{id}")]
    public async Task<IActionResult> DeletePost(long? id)
    {
      if (id != null)
      {
        BlogPost toRemove = _context.BlogPosts.Find(id);
        _context.BlogPosts.Remove(toRemove);
        await _context.SaveChangesAsync();
        return Redirect(Request.Headers["Referer"]);
      }
      return NotFound();
    }
  }
}

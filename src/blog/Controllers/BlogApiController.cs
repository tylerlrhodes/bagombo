using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using blog.Models;
using blog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace blog.Controllers
{
  [Route("api/[controller]/[action]")]
  public class BlogApiController : Controller
  {
    BlogContext _context;
    private readonly ILogger _logger;
    public BlogApiController(BlogContext context, ILogger<BlogApiController> logger)
    {
      _context = context;
      _logger = logger;
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int? id)
    {
      if (id != null)
      {
        BlogPost toRemove = _context.BlogPosts.Find(id);
        _context.BlogPosts.Remove(toRemove);
        await _context.SaveChangesAsync();
        return Ok(id);
      }
      return NotFound();
    }
    [HttpPost]
    public async Task<JsonResult> EditPost(BlogPost post)
    {
      if (ModelState.IsValid)
      {
        BlogPost update = _context.BlogPosts.Find(post.Id);
        update.Content = post.Content;
        update.Title = post.Title;
        await _context.SaveChangesAsync();
        return Json(update);
      }
      return Json(null);
    }
    [HttpPost]
    public async Task<JsonResult> AddPost(BlogPost post)
    {
      int id = (from c in _context.Authors
               where c.FirstName == "Tyler" && c.LastName == "Rhodes"
               select c.Id).FirstOrDefault();

      // Add Post to DB
      post.AuthorId = id;

      post.CreatedAt = DateTime.Now;

      _context.BlogPosts.Add(post);

      await _context.SaveChangesAsync();

      return Json(post);
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using blog.Models;
using blog.Data;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace blog.Controllers
{
  [Route("api/[controller]/[action]")]
  public class BlogController : Controller
  {
    BlogContext _context;
    public BlogController(BlogContext context)
    {
      _context = context;
    }
    [HttpPost]
    public async Task<JsonResult> AddPost(BlogPost post)
    {
      int id = _context.Authors
          .Where(c => c.FirstName == "Tyler" && c.LastName == "Rhodes")
          .Select(c => c.Id).FirstOrDefault();

      // Add Post to DB
      post.AuthorId = id;

      _context.BlogPosts.Add(post);

      await _context.SaveChangesAsync();

      var bp = _context.BlogPosts
          .Include(a => a.Author)
          .ToList();

      var authors = _context.Authors
          .Include(a => a.BlogPosts)
          .ToList();

      return Json(post);
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using blog.Models;
using blog.Models.ViewModels.Author;
using blog.Data;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace blog.Controllers
{
  [Authorize(Roles = "Authors")]
  public class AuthorController : Controller
  {
    BlogDbContext _context;
    UserManager<ApplicationUser> _userManager;

    public AuthorController(BlogDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }
    // GET: /<controller>/
    public IActionResult Index()
    {
      return View();
    }
    [HttpGet]
    public async Task<IActionResult> ManagePosts()
    {
      AuthorManagePostsViewModel ampvm = new AuthorManagePostsViewModel();

      var curUser = await _userManager.GetUserAsync(User);

      var author = (from u in _context.Users
                    where u.Id == curUser.Id
                    join a in _context.Authors on u.Id equals a.ApplicationUserId
                    select a).FirstOrDefault();

      var posts = (from bp in _context.BlogPosts
                   where bp.AuthorId == author.Id
                   select bp).AsEnumerable();

      ampvm.posts = posts;

      return View(ampvm);
    }
    [HttpGet]
    public IActionResult AddPost()
    {
      return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPost(AddBlogPostViewModel model)
    {
      if (ModelState.IsValid)
      {
        var curUser = await _userManager.GetUserAsync(User);
        var author = (from u in _userManager.Users
                      where u.Id == curUser.Id
                      join a in _context.Authors on u.Id equals a.ApplicationUserId
                      select a).FirstOrDefault();

        BlogPost bp = new BlogPost
        {
          Author = author,
          Title = model.Title,
          Content = model.Content,
          Description = model.Description,
          CreatedAt = DateTime.Now,
          ModifiedAt = DateTime.Now
        };

        await _context.BlogPosts.AddAsync(bp);
        await _context.SaveChangesAsync();

        return RedirectToAction("ManagePosts", "Author");
      }
      return View(model);
    }
    [HttpGet]
    public async Task<IActionResult> EditPost(int id)
    {
      //var post = (from bps in _context.BlogPosts
      //            where bps.Id == id
      //            select bps).FirstOrDefault();

      var post = await _context.BlogPosts.FindAsync(id);

      EditBlogPostViewModel ebpvm = new EditBlogPostViewModel
      {
        Title = post.Title,
        Content = post.Content,
        Description = post.Description
      };

      return View(ebpvm);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost()
    {
      return NotFound();
    }
  }

}

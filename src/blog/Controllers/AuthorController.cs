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
using CommonMark;

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
        try
        {
          await _context.BlogPosts.AddAsync(bp);
          await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
          ModelState.AddModelError("", "Error saving to database!");
          return View(model);
        }
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
        Id = id,
        Title = post.Title,
        Content = post.Content,
        Description = post.Description,
        PublishOn = DateTime.Now + TimeSpan.FromDays(2),
        Public = post.Public
      };

      return View(ebpvm);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(EditBlogPostViewModel model)
    {
      //var post = await _context.BlogPosts.FindAsync(model.Id);

      // fix these queries, the query for author can select from the author table without the join

      var post = _context.BlogPosts.Where(bp => bp.Id == model.Id).Include(bp => bp.Author).FirstOrDefault();
      var curUser = await _userManager.GetUserAsync(User);
      var author = (from u in _userManager.Users
                    where u.Id == curUser.Id
                    join a in _context.Authors on u.Id equals a.ApplicationUserId
                    select a).FirstOrDefault();

      // An admin is taking ownership
      if (post.Author == null)
        post.Author = author;

      post.Title = model.Title;
      post.Description = model.Description;
      post.Content = model.Content;
      post.ModifiedAt = DateTime.Now;
      post.PublishOn = model.PublishOn;
      post.Public = model.Public;

      await _context.SaveChangesAsync();

      //return RedirectToAction("ManagePosts");
      //return Redirect(Request.Headers["Referer"]);
      ViewData["SavedMessage"] = "Post saved.";

      return View(model); 
    }
    [HttpPost]
    public ContentResult GetPreviewHtml(string content)
    {
      return new ContentResult()
      {
        Content = CommonMark.CommonMarkConverter.Convert(content),
        ContentType = "text/html"
      };
    }
  }

}

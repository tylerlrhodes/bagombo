using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Bagombo.Models;
using Bagombo.Models.ViewModels.Author;
using Bagombo.EFCore;
using CommonMark;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Bagombo.Controllers
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

    public IActionResult Index()
    {
      return View();
    }

    [HttpGet]
    public async Task<IActionResult> ManagePosts()
    {
      AuthorManagePostsViewModel ampvm = new AuthorManagePostsViewModel();

      var curUser = await _userManager.GetUserAsync(User);

      var author = await (from u in _context.Users
                          where u.Id == curUser.Id
                          join a in _context.Authors on u.Id equals a.ApplicationUserId
                          select a).FirstOrDefaultAsync();

      var posts = await (from bp in _context.BlogPosts
                         where bp.AuthorId == author.Id
                         select bp).ToListAsync();

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
        var author = await (from u in _userManager.Users
                            where u.Id == curUser.Id
                            join a in _context.Authors on u.Id equals a.ApplicationUserId
                            select a).FirstOrDefaultAsync();

        BlogPost bp = new BlogPost
        {
          Author = author,
          Title = model.Title,
          Content = model.Content,
          Description = model.Description,
          CreatedAt = DateTime.Now,
          ModifiedAt = DateTime.Now,
          Public = false,
          PublishOn = DateTime.Now.AddDays(7)
        };
        try
        {
          _context.BlogPosts.Add(bp);
          await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
          ModelState.AddModelError("", "Error saving to database!");
          return View(model);
        }
        return RedirectToAction("EditPost", new { id = bp.Id });
      }
      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditPost(long id)
    {
      var post = await _context.BlogPosts.FindAsync(id);

      EditBlogPostViewModel ebpvm = new EditBlogPostViewModel
      {
        Id = id,
        Title = post.Title,
        Content = post.Content,
        Description = post.Description,
        PublishOn = post.PublishOn,
        Public = post.Public,
        FeaturesList = new List<FeaturesCheckBox>(),
        CategoriesList = new List<CategoriesCheckBox>()
      };

      var postHasCategories = await (from bpc in _context.BlogPostCategory
                                     where bpc.BlogPostId == id
                                     join c in _context.Categories on bpc.CategoryId equals c.Id
                                     select c).ToListAsync();

      foreach (var category in _context.Categories)
      {
        var categoryCheckBox = new CategoriesCheckBox()
        {
          CategoryId = category.Id,
          Name = category.Name,
          IsSelected = false
        };

        if (postHasCategories.Contains(category))
        {
          categoryCheckBox.IsSelected = true;
        }

        ebpvm.CategoriesList.Add(categoryCheckBox);
      }

      var postHasFeatures = await (from bpf in _context.BlogPostFeature
                                   where bpf.BlogPostId == id
                                   join f in _context.Features on bpf.FeatureId equals f.Id
                                   select f).ToListAsync();

      foreach (var feature in _context.Features)
      {
        var featureCheckBox = new FeaturesCheckBox()
        {
          FeatureId = feature.Id,
          Title = feature.Title,
          IsSelected = false
        };

        if (postHasFeatures.Contains(feature))
        {
          featureCheckBox.IsSelected = true;
        }

        ebpvm.FeaturesList.Add(featureCheckBox);
      }

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
      var author = await (from u in _userManager.Users
                          where u.Id == curUser.Id
                          join a in _context.Authors on u.Id equals a.ApplicationUserId
                          select a).FirstOrDefaultAsync();

      // An admin is taking ownership
      if (post.Author == null)
        post.Author = author;

      post.Title = model.Title;
      post.Description = model.Description;
      post.Content = model.Content;
      post.ModifiedAt = DateTime.Now;
      post.PublishOn = model.PublishOn;
      post.Public = model.Public;

      var bpfts = from bpf in _context.BlogPostFeature
                  where bpf.BlogPostId == post.Id
                  select bpf;

      _context.BlogPostFeature.RemoveRange(bpfts);

      await _context.SaveChangesAsync();

      if (model.FeaturesList != null)
      { 
        foreach (var feature in model.FeaturesList)
        {
          if (feature.IsSelected)
          {
            //var f = await _context.Features.FindAsync(feature.FeatureId);
            BlogPostFeature bpf = new BlogPostFeature()
            {
              //BlogPost = post,
              BlogPostId = post.Id,
              //Feature = f,
              FeatureId = feature.FeatureId
            };
            _context.BlogPostFeature.Add(bpf);
          }
        }
      }
      else
      {
        model.FeaturesList = new List<FeaturesCheckBox>();
      }
      var bpcts = from bpc in _context.BlogPostCategory
                  where bpc.BlogPostId == post.Id
                  select bpc;

      _context.BlogPostCategory.RemoveRange(bpcts);

      await _context.SaveChangesAsync();

      if (model.CategoriesList != null)
      {
        foreach (var category in model.CategoriesList)
        {
          if (category.IsSelected)
          {
            //var cat = await _context.Categories.FindAsync(category.CategoryId);

            BlogPostCategory bpc = new BlogPostCategory()
            {
              //BlogPost = post,
              BlogPostId = post.Id,
              //Category = cat,
              CategoryId = category.CategoryId
            };
            _context.BlogPostCategory.Add(bpc);
          }
        }
      }
      else
      {
        model.CategoriesList = new List<CategoriesCheckBox>();
      }

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

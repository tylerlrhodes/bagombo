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
using Bagombo.Data.Query;
using Bagombo.Data.Command;
using Bagombo.Data.Query.Queries;
using Bagombo.Data.Command.Commands;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Bagombo.Controllers
{
  [Authorize(Roles = "Authors")]
  public class AuthorController : Controller
  {
    private IQueryProcessorAsync _qpa;
    private ICommandProcessorAsync _cp;
    private UserManager<ApplicationUser> _userManager;

    public AuthorController(IQueryProcessorAsync qpa,
                            ICommandProcessorAsync cp,
                            UserManager<ApplicationUser> userManager)
    {
      _qpa = qpa;
      _cp = cp;
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

      var gbpbauid = new GetBlogPostsByAppUserIdQuery
      {
        AppUserId = curUser.Id
      };

      var posts = await _qpa.ProcessAsync(gbpbauid);

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

        var author = await _qpa.ProcessAsync(new GetAuthorByAppUserIdQuery { Id = curUser.Id });

        var abpc = new AddBlogPostCommand
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

        var result = await _cp.ProcessAsync(abpc);

        if (result.Succeeded)
        {
          return RedirectToAction("EditPost", new { id = result.Command.Id });
        }
        else
        {
          ModelState.AddModelError("", "Error saving to database!");
          return View(model);
        }
      }
      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditPost(long id)
    {
      var post = await _qpa.ProcessAsync(new GetBlogPostByIdQuery { Id = id });

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

      var postHasCategories = await _qpa.ProcessAsync(new GetCategoriesForBlogPostByIdQuery { Id = id });

      foreach (var category in await _qpa.ProcessAsync(new GetCategoriesQuery()))
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

      var postHasFeatures = await _qpa.ProcessAsync(new GetTopicsForBlogPostByIdQuery { Id = id });

      foreach (var feature in await _qpa.ProcessAsync(new GetTopicsQuery()))
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
      // fix these queries, the query for author can select from the author table without the join

      var post = await _qpa.ProcessAsync(new GetBlogPostByIdQuery { Id = model.Id });

      var ubpc = new UpdateBlogPostCommand();

      if (post.Author == null)
      {
        var curUser = await _userManager.GetUserAsync(User);

        var author = await _qpa.ProcessAsync(new GetAuthorByAppUserIdQuery { Id = curUser.Id });

        ubpc.NewAuthor = author;
      }
      else
      { 
        ubpc.NewAuthor = post.Author; // keep the same author
      }

      ubpc.Id = model.Id;
      ubpc.NewTitle = model.Title;
      ubpc.NewDescription = model.Description;
      ubpc.NewContent = model.Content;
      ubpc.LastModifiedAt = DateTime.Now;
      ubpc.NewPublishOn = model.PublishOn;
      ubpc.NewPublic = model.Public;

      // update the post

      var ubpcResult = await _cp.ProcessAsync(ubpc);

      if (ubpcResult.Succeeded)
      {
        // do nothing
      }
      else
      {
        // an error
        return NotFound();
      }

      List<long> featureIds = new List<long>();

      if (model.FeaturesList != null)
      {
        foreach (var feature in model.FeaturesList)
        {
          if (feature.IsSelected)
          {
            featureIds.Add(feature.FeatureId);
          }
        }

        var addFeaturesResult = 
          await _cp.ProcessAsync(new SetBlogPostTopicsCommand
          {
            BlogPostId = post.Id,
            TopicIds = featureIds
          });

        if (addFeaturesResult.Succeeded)
        {
          // do nothing
        }
        else
        {
          // log the error
          return NotFound();
        }
      }
      else
      {
        model.FeaturesList = new List<FeaturesCheckBox>();
      }

      List<long> categoryIds = new List<long>();

      if (model.CategoriesList != null)
      {
        foreach (var category in model.CategoriesList)
        {
          if (category.IsSelected)
          {
            categoryIds.Add(category.CategoryId);
          }
        }

        var addCategoriesResult =
          await _cp.ProcessAsync(new SetBlogPostCategoriesCommand
          {
            BlogPostId = post.Id,
            CategoryIds = categoryIds
          });

        if (addCategoriesResult.Succeeded)
        {
          // do nothing
        }
        else
        {
          // log the error
          return NotFound();
        }
      }
      else
      {
        model.CategoriesList = new List<CategoriesCheckBox>();
      }

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

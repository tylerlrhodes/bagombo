using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bagombo.Data.Command;
using Bagombo.Data.Command.Commands;
using Bagombo.Data.Query;
using Bagombo.Data.Query.Queries;
using Bagombo.Models.ViewModels.Home;
using CommonMark;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TylerRhodes.Akismet;

namespace Bagombo.Controllers
{
  public class HomeController : Controller
  {
    private readonly AkismetClient _akismetClient;
    private readonly ICommandProcessorAsync _cp;
    private readonly ILogger _logger;
    private readonly IQueryProcessorAsync _qpa;
    private readonly BagomboSettings _settings;

    public HomeController(IQueryProcessorAsync qpa,
      ICommandProcessorAsync cp,
      ILogger<HomeController> logger,
      IOptions<BagomboSettings> options,
      AkismetClient akismetClient)
    {
      _logger = logger;
      _qpa = qpa;
      _cp = cp;
      _settings = options.Value;
      _akismetClient = akismetClient;
    }

    public async Task<IActionResult> Index(int? page)
    {
      var curPage = page ?? 1;

      var gbpfhp = new GetBlogPostsForHomePageQuery
      {
        CurrentPage = curPage,
        PageSize = _settings.PostsOnHomePage
      };

      var recentPosts = await _qpa.ProcessAsync(gbpfhp);

      var categories = await _qpa.ProcessAsync(new GetCategoriesQuery());

      var vhvm = new HomeViewModel
      {
        Categories = categories,
        RecentPosts = recentPosts
      };

      return View(vhvm);
    }

    public async Task<IActionResult> Search(string searchText)
    {
      var search = "\"*" + searchText + "*\"";

      var gbpbst = new GetSearchResultBlogPostsBySearchTextViewModelQuery
      {
        SearchText = search
      };

      IEnumerable<SearchResultBlogPostViewModel> bps = null;

      try
      {
        bps = await _qpa.ProcessAsync(gbpbst);
      }
      catch (Exception e)
      {
        _logger.LogError(0, e, "Exception during search!");

        bps = new List<SearchResultBlogPostViewModel>();
      }

      var vsrvm = new SearchResultsViewModel
      {
        SearchTerm = searchText,
        BlogPosts = bps
      };

      return View(vsrvm);
    }

    public async Task<IActionResult> CategoryPosts(long id)
    {
      var gvcpbc = new GetCategoryPostsByCategoryViewModelQuery
      {
        Id = id
      };

      var vcpvm = await _qpa.ProcessAsync(gvcpbc);

      if (vcpvm != null)
        return View(vcpvm);

      _logger.LogWarning("Warning - CategoryPosts called with invalid Category Id {0}", id);

      return NotFound();
    }

    public async Task<IActionResult> AllPosts(int? sortby = 1)
    {
      var vm = new AllPostsViewModel();

      // Sort by Category
      if (sortby == 1)
      {
        var gvapbc = new GetAllPostsByCategoryViewModelQuery();

        vm = await _qpa.ProcessAsync(gvapbc);
      }
      // return sorted by date
      else
      {
        var gvapbd = new GetAllPostsByDateViewModelQuery();

        vm = await _qpa.ProcessAsync(gvapbd);
      }

      return View(vm);
    }

    public async Task<IActionResult> TopicPosts(long id)
    {
      var gvfpbf = new GetTopicPostsByTopicViewModelQuery
      {
        Id = id
      };

      var vfpvm = await _qpa.ProcessAsync(gvfpbf);

      if (vfpvm == null)
      {
        _logger.LogWarning("Warning - TopicPosts called with a Topic Id that returned null from query. Topic Id = {0}",
          id);

        return NotFound();
      }

      return View(vfpvm);
    }

    public async Task<IActionResult> Topics()
    {
      var gvf = new GetTopicsViewModelQuery();

      var vfvm = await _qpa.ProcessAsync(gvf);

      return View(vfvm);
    }

    [Authorize(Roles = "Admins")]
    public async Task<IActionResult> DeleteComment(long id, long blogPostId)
    {
      var result = await _cp.ProcessAsync(new DeleteCommentCommand {Id = id});

      if (!result.Succeeded)
        _logger.LogError($"Error deleting comment with Id = {id}");

      return RedirectToAction(nameof(BlogPost), new {id = blogPostId});
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(AddCommentViewModel model)
    {
      if (!ModelState.IsValid) return RedirectToAction(nameof(BlogPostBySlug), new {slug = model.Slug});

      if (model.Text != null)
      {
        model.Text = model.Text.Replace("<", "&lt;");
        model.Text = model.Text.Replace(">", "&gt;");
      }

      // validate comment isn't spam
      var comment = new AkismetComment
      {
        UserIp = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
        UserAgent = Request.Headers["User-Agent"].ToString(),
        Author = model.Name,
        AuthorEmail = model.Email,
        AuthorUrl = model.Website,
        CommentType = "comment",
        Content = model.Text,
        Permalink = _akismetClient.BlogUrl + Url.Action(nameof(BlogPostBySlug), new {slug = model.Slug})
      };

      if (await _akismetClient.IsCommentSpam(comment))
        return RedirectToAction(nameof(BlogPostBySlug), new {slug = model.Slug});

      var addComment = new AddCommentCommand
      {
        Name = model.Name,
        Email = model.Email,
        Website = model.Website,
        Text = model.Text,
        BlogPostId = model.Id
      };

      var result = await _cp.ProcessAsync(addComment);

      if (result.Succeeded)
        return RedirectToAction(nameof(BlogPostBySlug), new {slug = model.Slug});
      _logger.LogError("Error adding comment...");

      return RedirectToAction(nameof(BlogPostBySlug), new {slug = model.Slug});
    }

    [Route("blog/{slug}")]
    public async Task<IActionResult> BlogPostBySlug(string slug)
    {
      if (slug == null)
      {
        _logger.LogWarning("Warning - BlogPost with null slug called.");

        return NotFound();
      }

      var gbpbs = new GetBlogPostBySlugViewModelQuery
      {
        Slug = slug
      };

      var bpvm = await _qpa.ProcessAsync(gbpbs);

      if (bpvm == null)
      {
        _logger.LogWarning("Warning - BlogPost with slug {0} was not found.", slug);

        return NotFound();
      }

      if (!bpvm.IsHtml) bpvm.Content = CommonMarkConverter.Convert(bpvm.Content);

      return View("BlogPost", bpvm);
    }

    public async Task<IActionResult> BlogPost(long? id)
    {
      if (id == null)
      {
        _logger.LogWarning("Warning - BlogPost with null Id called.");

        return NotFound();
      }

      var gvbpbi = new GetBlogPostByIdViewModelQuery
      {
        Id = (long) id
      };

      var bpvm = await _qpa.ProcessAsync(gvbpbi);

      if (bpvm == null)
      {
        _logger.LogWarning("Warning - BlogPost with Id {0} was not found.", id);

        return NotFound();
      }

      if (!bpvm.IsHtml) bpvm.Content = CommonMarkConverter.Convert(bpvm.Content);

      return View(bpvm);
    }
  }
}
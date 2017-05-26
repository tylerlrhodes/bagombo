using Bagombo.Data.Query;
using Bagombo.Data.Query.Queries;
using Bagombo.EFCore;
using Bagombo.Models;
using Bagombo.Models.ViewModels.Home;
using CommonMark;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Bagombo.Controllers
{
  public class HomeController : Controller
  {
    private IQueryProcessorAsync _qpa;
    private ILogger _logger;

    public HomeController(IQueryProcessorAsync qpa, ILogger<HomeController> logger)
    {
      _logger = logger;
      _qpa = qpa;
    }

    public async Task<IActionResult> Index()
    {
      var grbp = new GetRecentBlogPostsQuery()
      {
        NumberOfPostsToGet = 7
      };

      var recentPosts = await _qpa.ProcessAsync(grbp);

      var vhvm = new HomeViewModel()
      {
        RecentPosts = recentPosts
      };

      return View(vhvm);
    }

    public async Task<IActionResult> Search(string searchText)
    {
      var search = "\"*" + searchText + "*\"";

      var gbpbst = new GetSearchResultBlogPostsBySearchTextViewModelQuery()
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

      var vsrvm = new SearchResultsViewModel()
      {
        SearchTerm = searchText,
        BlogPosts = bps
      };

      return View(vsrvm);
    }

    public async Task<IActionResult> CategoryPosts(long id)
    {
      var gvcpbc = new GetCategoryPostsByCategoryViewModelQuery()
      {
        Id = (long)id
      };

      var vcpvm = await _qpa.ProcessAsync(gvcpbc);

      if (vcpvm != null)
      {
        return View(vcpvm);
      }
      else
      {
        _logger.LogWarning("Warning - CategoryPosts called with invalid Category Id {0}", id);

        return NotFound();
      }
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
      var gvfpbf = new GetTopicPostsByTopicViewModelQuery()
      {
        Id = id
      };

      var vfpvm = await _qpa.ProcessAsync(gvfpbf);

      if (vfpvm == null)
      {
        _logger.LogWarning("Warning - TopicPosts called with a Topic Id that returned null from query. Topic Id = {0}", id);

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

    public IActionResult About()
    {
      return View();
    }

    public async Task<IActionResult> BlogPost(long? id)
    {
      if (id == null)
      {
        _logger.LogWarning("Warning - BlogPost with null Id called.");

        return NotFound();
      }

      var gvbpbi = new GetBlogPostByIdViewModelQuery()
      {
        Id = (long)id
      };

      var bpvm = await _qpa.ProcessAsync(gvbpbi);

      if (bpvm == null)
      {
        _logger.LogWarning("Warning - BlogPost with Id {0} was not found.", id);

        return NotFound();
      }

      bpvm.Content = CommonMarkConverter.Convert(bpvm.Content);

      return View(bpvm);
    }

  }

}

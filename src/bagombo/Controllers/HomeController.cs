using Bagombo.Data.Query;
using Bagombo.Data.Query.Queries;
using Bagombo.EFCore;
using Bagombo.Models;
using Bagombo.Models.ViewModels.Home;
using CommonMark;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public HomeController(IQueryProcessorAsync qpa)
    {
      _qpa = qpa;
    }

    public async Task<IActionResult> Index()
    {
      GetRecentBlogPosts grbp = new GetRecentBlogPosts()
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

      GetSearchResultBlogPostsBySearchTextViewModel gbpbst = new GetSearchResultBlogPostsBySearchTextViewModel()
      {
        searchText = search
      };

      var bps = await _qpa.ProcessAsync(gbpbst);

      var vsrvm = new SearchResultsViewModel()
      {
        SearchTerm = searchText,
        BlogPosts = bps
      };

      return View(vsrvm);
    }

    public async Task<IActionResult> CategoryPosts(long? id)
    {
      GetCategoryPostsByCategoryViewModel gvcpbc = new GetCategoryPostsByCategoryViewModel()
      {
        Id = (long)id
      };

      var vcpvm = await _qpa.ProcessAsync(gvcpbc);

      return View(vcpvm);
    }

    public async Task<IActionResult> AllPosts(int? sortby = 1)
    {
      var vm = new AllPostsViewModel();

      // Sort by Category
      if (sortby == 1)
      {

        GetAllPostsByCategoryViewModel gvapbc = new GetAllPostsByCategoryViewModel();

        vm = await _qpa.ProcessAsync(gvapbc);

      }
      // return sorted by date
      else
      {
        GetAllPostsByDateViewModel gvapbd = new GetAllPostsByDateViewModel();

        vm = await _qpa.ProcessAsync(gvapbd);
      }

      return View(vm);
    }

    public async Task<IActionResult> FeaturePosts(long id)
    {
      GetFeaturePostsByFeatureViewModel gvfpbf = new GetFeaturePostsByFeatureViewModel()
      {
        Id = id
      };

      var vfpvm = await _qpa.ProcessAsync(gvfpbf);

      if (vfpvm == null)
      {
        return NotFound();
      }

      return View(vfpvm);
    }

    public async Task<IActionResult> Features()
    {
      GetFeaturesViewModel gvf = new GetFeaturesViewModel();

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
        return NotFound();

      GetBlogPostByIdViewModel gvbpbi = new GetBlogPostByIdViewModel()
      {
        Id = (long)id
      };

      var bpvm = await _qpa.ProcessAsync(gvbpbi);

      if (bpvm == null)
        return NotFound();

      bpvm.Content = CommonMarkConverter.Convert(bpvm.Content);

      return View(bpvm);
    }

  }

}

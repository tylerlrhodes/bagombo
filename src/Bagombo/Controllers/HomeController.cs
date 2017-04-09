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
      GetRecentBlogPostsQuery grbp = new GetRecentBlogPostsQuery()
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

      GetSearchResultBlogPostsBySearchTextViewModelQuery gbpbst = new GetSearchResultBlogPostsBySearchTextViewModelQuery()
      {
        SearchText = search
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
      GetCategoryPostsByCategoryViewModelQuery gvcpbc = new GetCategoryPostsByCategoryViewModelQuery()
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

        GetAllPostsByCategoryViewModelQuery gvapbc = new GetAllPostsByCategoryViewModelQuery();

        vm = await _qpa.ProcessAsync(gvapbc);

      }
      // return sorted by date
      else
      {
        GetAllPostsByDateViewModelQuery gvapbd = new GetAllPostsByDateViewModelQuery();

        vm = await _qpa.ProcessAsync(gvapbd);
      }

      return View(vm);
    }

    public async Task<IActionResult> FeaturePosts(long id)
    {
      GetFeaturePostsByFeatureViewModelQuery gvfpbf = new GetFeaturePostsByFeatureViewModelQuery()
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
      GetFeaturesViewModelQuery gvf = new GetFeaturesViewModelQuery();

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

      GetBlogPostByIdViewModelQuery gvbpbi = new GetBlogPostByIdViewModelQuery()
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

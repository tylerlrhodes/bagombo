using blog.data.Query;
using blog.data.Query.Queries;
using blog.EFCore;
using blog.Models;
using blog.Models.ViewModels.Home;
using CommonMark;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace blog.Controllers
{
  public class HomeController : Controller
  {
    BlogDbContext _context;
    QueryProcessorAsync _qpa;

    public HomeController(BlogDbContext context, QueryProcessorAsync qpa)
    {
      _context = context;
      _qpa = qpa;
    }

    public async Task<IActionResult> Index()
    {
      GetRecentBlogPosts grbp = new GetRecentBlogPosts()
      {
        NumberOfPostsToGet = 7
      };

      var recentPosts = await _qpa.ProcessAsync(grbp);

      var vhvm = new ViewHomeViewModel()
      {
        RecentPosts = recentPosts
      };

      return View(vhvm);
    }

    public async Task<IActionResult> Search(string searchText)
    {
      var search = "\"*" + searchText + "*\"";

      GetViewSearchResultBlogPostsBySearchText gbpbst = new GetViewSearchResultBlogPostsBySearchText()
      {
        searchText = search
      };

      var bps = await _qpa.ProcessAsync(gbpbst);

      var vsrvm = new ViewSearchResultsViewModel()
      {
        SearchTerm = searchText,
        BlogPosts = bps
      };

      return View(vsrvm);
    }

    public async Task<IActionResult> CategoryPosts(long? id)
    {
      GetViewCategoryPostsByCategory gvcpbc = new GetViewCategoryPostsByCategory()
      {
        Id = (long)id
      };

      var vcpvm = await _qpa.ProcessAsync(gvcpbc);

      return View(vcpvm);
    }

    public async Task<IActionResult> AllPosts(int? sortby = 1)
    {
      var vm = new ViewAllPostsViewModel();

      // Sort by Category
      if (sortby == 1)
      {

        GetViewAllPostsByCategory gvapbc = new GetViewAllPostsByCategory();

        vm = await _qpa.ProcessAsync(gvapbc);

      }
      // return sorted by date
      else
      {
        GetViewAllPostsByDate gvapbd = new GetViewAllPostsByDate();

        vm = await _qpa.ProcessAsync(gvapbd);
      }

      return View(vm);
    }

    public async Task<IActionResult> FeaturePosts(long id)
    {
      GetViewFeaturePostsByFeature gvfpbf = new GetViewFeaturePostsByFeature()
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
      GetViewFeatures gvf = new GetViewFeatures();

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

      GetViewBlogPostById gvbpbi = new GetViewBlogPostById()
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

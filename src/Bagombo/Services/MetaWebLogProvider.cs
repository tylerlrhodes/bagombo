using Bagombo.Data.Command;
using Bagombo.Data.Command.Commands;
using Bagombo.Data.Query.Queries;
using Bagombo.Data.Query;
using Bagombo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WilderMinds.MetaWeblog;

namespace Bagombo.Services
{
  public class MetaWebLogProvider : IMetaWeblogProvider
  {
    private readonly IQueryProcessorAsync _qp;
    private readonly ICommandProcessorAsync _cp;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IHttpContextAccessor _context;

    public MetaWebLogProvider(IQueryProcessorAsync qp,
                              ICommandProcessorAsync cp,
                              UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IHttpContextAccessor context)
    {
      _qp = qp;
      _cp = cp;
      _userManager = userManager;
      _signInManager = signInManager;
      _context = context;
    }

    private bool validateUser(string username, string password)
    {
      var user = _userManager.Users.Where(u => u.Email == username).FirstOrDefault();

      var result = _signInManager.PasswordSignInAsync(user, password, false, false).GetAwaiter().GetResult();

      if (result.Succeeded)
      {
        return true;
      }

      return false;
    }

    private Author getAuthor(string username)
    {
      return _qp.ProcessAsync(new GetAuthorByAppUserIdQuery
      {
        Id = _userManager.Users.Where(u => u.Email == username).FirstOrDefault().Id
      }).GetAwaiter().GetResult();
    }

    public int AddCategory(string key, string username, string password, NewCategory category)
    {
      throw new NotImplementedException();
    }

    public string AddPost(string blogid, string username, string password, Post post, bool publish)
    {
      if (validateUser(username, password))
      {
        var author = getAuthor(username);
        var np = new AddBlogPostCommand()
        {
          Title = post.title,
          Author = author,
          Description = "Uploaded post",
          Content = post.description,
          CreatedAt = post.dateCreated,
          ModifiedAt = DateTime.Now,
          Public = false,
          PublishOn = DateTime.Now
        };
        var result = _cp.ProcessAsync(np).GetAwaiter().GetResult();
        if (result.Succeeded)
        {
          return result.Command.Id.ToString();
        }
      }
      return "-1";
    }

    public bool DeletePost(string key, string postid, string username, string password, bool publish)
    {
      throw new NotImplementedException();
    }

    public bool EditPost(string postid, string username, string password, Post post, bool publish)
    {
      return true;
    }

    public CategoryInfo[] GetCategories(string blogid, string username, string password)
    {
      throw new NotImplementedException();
    }

    public Post GetPost(string postid, string username, string password)
    {

      return new Post
      {
        title = "Test"
      };
    }

    public Post[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
    {
      throw new NotImplementedException();
    }

    public UserInfo GetUserInfo(string key, string username, string password)
    {
      throw new NotImplementedException();
    }

    public BlogInfo[] GetUsersBlogs(string key, string username, string password)
    {
      throw new NotImplementedException();
    }

    public MediaObjectInfo NewMediaObject(string blogid, string username, string password, MediaObject mediaObject)
    {
      throw new NotImplementedException();
    }
  }
}

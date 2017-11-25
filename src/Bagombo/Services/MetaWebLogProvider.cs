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
using Microsoft.Extensions.Options;

namespace Bagombo.Services
{
  public class MetaWebLogProvider : IMetaWeblogProvider
  {
    private readonly IQueryProcessorAsync _qp;
    private readonly ICommandProcessorAsync _cp;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IHttpContextAccessor _context;
    private readonly BagomboSettings _settings;

    public MetaWebLogProvider(IQueryProcessorAsync qp,
                              ICommandProcessorAsync cp,
                              UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IOptions<BagomboSettings> settings,
                              IHttpContextAccessor context)
    {
      _qp = qp;
      _cp = cp;
      _userManager = userManager;
      _signInManager = signInManager;
      _settings = settings.Value;
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
      })
      .GetAwaiter()
      .GetResult();
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
          Description = post.mt_excerpt,
          Content = post.description,
          CreatedAt = post.dateCreated,
          ModifiedAt = DateTime.Now,
          Public = publish,
          PublishOn = DateTime.Now
        };

        var result = _cp.ProcessAsync(np).GetAwaiter().GetResult();

        if (result.Succeeded)
        {
          if (post.categories.Count() > 0)
          { 
            var sc = new SetBlogPostCategoriesByStringArrayCommand
            {
              Categories = post.categories.ToList(),
              BlogPostId = result.Command.Id
            };

            var scResult = _cp.ProcessAsync(sc).GetAwaiter().GetResult();

            if (scResult.Succeeded)
            {
              return result.Command.Id.ToString();
            }
          }
          else
          {
            return result.Command.Id.ToString();
          }
        }
      }
      return "-1";
    }

    public bool DeletePost(string key, string postid, string username, string password, bool publish)
    {
      if (validateUser(username, password))
      {
        if (long.TryParse(postid, out var pid))
        {
          var result = _cp.ProcessAsync(new DeleteBlogPostCommand { Id = pid }).GetAwaiter().GetResult();

          if (result.Succeeded)
          {
            return true;
          }
        }
      }
      return false;
    }

    public bool EditPost(string postid, string username, string password, Post post, bool publish)
    {
      if (validateUser(username, password))
      {
        if (long.TryParse(postid, out var id))
        {
          var author = getAuthor(username);

          var up = new UpdateBlogPostCommand
          {
            Id = id,
            NewTitle = post.title,
            NewAuthor = author,
            NewContent = post.description,
            NewDescription = post.mt_excerpt,
            NewPublic = publish,
            NewPublishOn = publish ? DateTime.Now : DateTime.Now.AddDays(365),
            LastModifiedAt = DateTime.Now
          };

          var result = _cp.ProcessAsync(up).GetAwaiter().GetResult();

          if (result.Succeeded)
          {
            if(post.categories.Count() > 0)
            { 
              var sc = new SetBlogPostCategoriesByStringArrayCommand
              {
                Categories = post.categories.ToList(),
                BlogPostId = result.Command.Id
              };

              var scResult = _cp.ProcessAsync(sc).GetAwaiter().GetResult();

              if (scResult.Succeeded)
              {
                return true;
              }
              else
              {
                return false;
              }
            }
            else
            {
              return true;
            }
          }
        }
      }
      return false;
    }

    public CategoryInfo[] GetCategories(string blogid, string username, string password)
    {
      if (validateUser(username, password))
      {
        var categories = _qp.ProcessAsync(new GetCategoriesQuery()).GetAwaiter().GetResult();

        var catInfoList = new List<CategoryInfo>();

        foreach (var category in categories)
        {
          catInfoList.Add(new CategoryInfo()
          {
            categoryid = category.Id.ToString(),
            title = category.Name
          });
        }

        return catInfoList.ToArray();
      }
      return null;
    }

    public Post GetPost(string postid, string username, string password)
    {
      if (validateUser(username, password))
      {
        if (long.TryParse(postid, out var pid))
        {
          var bp = _qp.ProcessAsync(new GetBlogPostByIdQuery { Id = pid }).GetAwaiter().GetResult();

          if (bp != null)
          {
            var cats = bp.BlogPostCategory?.Select(c => c.Category.Name)?.ToArray();
            var url = _context.HttpContext.Request.Scheme + "://" + _context.HttpContext.Request.Host + bp.GetUrl();

            return new Post
            {
              categories = cats ?? new string[] { "" },
              title = bp.Title,
              dateCreated = bp.CreatedAt,
              description = bp.Content,
              mt_excerpt = bp.Description,
              postid = bp.Id.ToString(),
              link = url,
              permalink = url,
              userid = username
            };
          }
        }
      }
      return null;
    }

    public Post[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
    {
      if (validateUser(username, password))
      {
        var bps = _qp.ProcessAsync(new GetRecentBlogPostsQuery { NumberOfPostsToGet = numberOfPosts }).GetAwaiter().GetResult();

        if (bps != null)
        {
          var postList = new List<Post>();

          foreach (var bp in bps)
          {
            var cats = bp.BlogPostCategory?.Select(c => c.Category.Name)?.ToArray();
            var url = _context.HttpContext.Request.Scheme + "://" + _context.HttpContext.Request.Host + bp.GetUrl();

            var post = new Post
            {
              categories = cats ?? new string[] { "" },
              title = bp.Title,
              dateCreated = bp.CreatedAt,
              description = bp.Content,
              mt_excerpt = bp.Description,
              postid = bp.Id.ToString(),
              link = url,
              permalink = url,
              userid = username
            };

            postList.Add(post);
          }

          return postList.ToArray();
        }
      }
      return null;
    }

    public UserInfo GetUserInfo(string key, string username, string password)
    {
      throw new NotImplementedException();
    }

    public BlogInfo[] GetUsersBlogs(string key, string username, string password)
    {
      return new BlogInfo[]
      {
        new BlogInfo
        {
          blogid = "1",
          blogName = _settings.Brand,
          url = _context.HttpContext.Request.Scheme + "://" + _context.HttpContext.Request.Host
        }
      };
    }

    public MediaObjectInfo NewMediaObject(string blogid, string username, string password, MediaObject mediaObject)
    {
      throw new NotImplementedException();
    }
  }
}

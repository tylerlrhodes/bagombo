using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bagombo.Data.Query;
using Bagombo.Data.Query.Queries;
using Microsoft.Extensions.Logging;
using System.Text;
using Bagombo.Models;
using System.Xml;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using Microsoft.SyndicationFeed.Atom;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CommonMark;

namespace Bagombo.Controllers
{
  public class MetaController : Controller
  {
    private readonly IQueryProcessorAsync _qp;
    private readonly ILogger _logger;
    private readonly BagomboSettings _settings;
    private readonly UserManager<ApplicationUser> _userManager;

    public MetaController(IQueryProcessorAsync qp,
                          ILogger<MetaController> logger,
                          IOptions<BagomboSettings> settings,
                          UserManager<ApplicationUser> userManager)
    {
      _qp = qp;
      _logger = logger;
      _settings = settings.Value;
      _userManager = userManager;
    }

    [Route("/robots.txt")]
    public string RobotsDotTxt()
    {
      var host = Request.Scheme + "://" + Request.Host;
      var sb = new StringBuilder();
      sb.AppendLine("User-Agent: *");
      sb.AppendLine("Disallow: /admin/");
      sb.AppendLine("Disallow: /account/");
      sb.AppendLine("Disallow: /author/");
      sb.AppendLine($"sitemap: {host}/sitemap.xml");
      return sb.ToString();
    }

    [Route("/rsd.xml")]
    public void RsdXml()
    {
      string host = Request.Scheme + "://" + Request.Host;

      Response.ContentType = "application/xml";
      Response.Headers["cache-control"] = "no-cache, no-store, must-revalidate";

      using (var xml = XmlWriter.Create(Response.Body, new XmlWriterSettings { Indent = true }))
      {
        xml.WriteStartDocument();
        xml.WriteStartElement("rsd");
        xml.WriteAttributeString("version", "1.0");

        xml.WriteStartElement("service");

        xml.WriteElementString("enginename", "Bagombo Blog Engine");
        xml.WriteElementString("enginelink", "http://github.com/tylerlrhodes/bagombo/");
        xml.WriteElementString("homepagelink", host);

        xml.WriteStartElement("apis");
        xml.WriteStartElement("api");
        xml.WriteAttributeString("name", "MetaWeblog");
        xml.WriteAttributeString("preferred", "true");
        xml.WriteAttributeString("apilink", host + "/metaweblog");
        xml.WriteAttributeString("blogid", "1");

        xml.WriteEndElement(); // api
        xml.WriteEndElement(); // apis
        xml.WriteEndElement(); // service
        xml.WriteEndElement(); // rsd
      }
    }

    [Route("/sitemap.xml")]
    public async Task SitemapXml()
    {
      var host = Request.Scheme + "://" + Request.Host;

      Response.ContentType = "application/xml";

      using (var xml = XmlWriter.Create(Response.Body, new XmlWriterSettings { Indent = true }))
      {
        xml.WriteStartDocument();
        xml.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

        var blogPosts = await _qp.ProcessAsync(new GetBlogPostsQuery());

        foreach (var bp in blogPosts)
        {
          xml.WriteStartElement("url");
          xml.WriteElementString("loc", host + bp.GetUrl());
          xml.WriteElementString("lastmod", bp.ModifiedAt.ToString("yyyy-MM-ddThh:mmzzz"));
          xml.WriteEndElement();
        }

        xml.WriteEndElement();
      }
    }

    [Route("/feed/{type}")]
    public async Task Feed(string type)
    {
      var host = Request.Scheme + "://" + Request.Host;
      Response.ContentType = "application/xml";

      using (var xmlWriter = XmlWriter.Create(Response.Body, new XmlWriterSettings { Async = true, Indent = true }))
      {
        var blogPosts = await _qp.ProcessAsync(new GetBlogPostsQuery());
        var writer = await GetWriter(type, xmlWriter, blogPosts.Max(bp => bp.ModifiedAt));

        foreach (var bp in blogPosts)
        {
          var item = new AtomEntry
          {
            Title = bp.Title,
            Description = type.Equals("rss", StringComparison.OrdinalIgnoreCase) ? bp.Description 
              : CommonMarkConverter.Convert(bp.Content),
            Id = host + bp.GetUrl(),
            Published = bp.PublishOn,
            LastUpdated = bp.ModifiedAt,
            ContentType = "html"
          };

          foreach (var c in bp.BlogPostCategory.Select(bpc => bpc.Category).ToList())
          {
            item.AddCategory(new SyndicationCategory(c.Name));
          }

          var user = await _userManager.Users.Where(u => u.Id == bp.Author.ApplicationUserId).FirstOrDefaultAsync();
          var email = user?.Email ?? _settings.ContactEmail;

          item.AddContributor(new SyndicationPerson(bp.Author.FirstName + " " + bp.Author.LastName, email));
          item.AddLink(new SyndicationLink(new Uri(item.Id)));

          await writer.Write(item);
        }
      }
    }

    private async Task<ISyndicationFeedWriter> GetWriter(string type, XmlWriter xmlWriter, DateTime updated)
    {
      var host = Request.Scheme + "://" + Request.Host + "/";

      if (type.Equals("rss", StringComparison.OrdinalIgnoreCase))
      {
        var rss = new RssFeedWriter(xmlWriter);
        await rss.WriteTitle(_settings.Brand);
        await rss.WriteDescription(_settings.Description);
        await rss.WriteGenerator("Bagombo Blog Engine");
        await rss.WriteValue("link", host);
        return rss;
      }

      var atom = new AtomFeedWriter(xmlWriter);
      await atom.WriteTitle(_settings.Brand);
      await atom.WriteId(host);
      await atom.WriteSubtitle(_settings.Description);
      await atom.WriteGenerator("Bagombo Blog Engine", "https://github.com/tylerlrhodes/bagobo", "0.2.5a");
      await atom.WriteValue("updated", updated.ToString("yyyy-MM-ddTHH:mm:ssZ"));
      return atom;
    }

  }
}

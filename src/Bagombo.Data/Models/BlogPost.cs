using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bagombo.Models
{
  public class BlogPost
  {
    public long Id { get; set; }
    public long? AuthorId { get; set; }
    public Author Author { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public DateTime PublishOn { get; set; }
    public bool Public { get; set; }
    public ICollection<BlogPostCategory> BlogPostCategory { get; set; }
    public ICollection<BlogPostTopic> BlogPostTopic { get; set; }
    public string Image { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public bool IsHtml { get; set; }
  }

  public static class BlogPostExtensions
  {
    public static string GetUrl(this BlogPost bp)
    {
      //return $"/Home/BlogPost/{bp.Id}";
      return $"/blog/{bp.Slug}";
    }

    public static string CreateSlug(string title)
    {
      title = title.ToLowerInvariant().Replace(" ", "-");
      title = RemoveDiacritics(title);
      title = RemoveReservedUrlCharacters(title);

      return title.ToLowerInvariant();
    }

    static string RemoveReservedUrlCharacters(string text)
    {
      var reservedCharacters = new List<string> { "!", "#", "$", "&", "'", "(", ")", "*", ",", "/", ":", ";", "=", "?", "@", "[", "]", "\"", "%", ".", "<", ">", "\\", "^", "_", "'", "{", "}", "|", "~", "`", "+" };

      foreach (var chr in reservedCharacters)
      {
        text = text.Replace(chr, "");
      }

      return text;
    }

    static string RemoveDiacritics(string text)
    {
      var normalizedString = text.Normalize(NormalizationForm.FormD);
      var stringBuilder = new StringBuilder();

      foreach (var c in normalizedString)
      {
        var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
        if (unicodeCategory != UnicodeCategory.NonSpacingMark)
        {
          stringBuilder.Append(c);
        }
      }

      return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
  }
}

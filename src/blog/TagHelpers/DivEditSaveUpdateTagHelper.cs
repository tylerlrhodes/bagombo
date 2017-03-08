using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace blog.TagHelpers
{
  [HtmlTargetElement("div", Attributes = "SaveUpdate")]
  public class DivEditSaveUpdateTagHelper : TagHelper
  {
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      string x = (string)ViewContext.ViewData["SavedMessage"];

      var tagBuilder = new TagBuilder("div");

      if (x != null)
      { 
        tagBuilder.AddCssClass("alert alert-success");

        output.MergeAttributes(tagBuilder);
        output.Content.SetContent(x);
      }

      base.Process(context, output);
    }
  }
}

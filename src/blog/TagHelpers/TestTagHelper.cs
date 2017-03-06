using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace blog.TagHelpers
{
  [HtmlTargetElement(Attributes= "SavedMessage")]
  public class TestTagHelper : TagHelper
  {
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      var x = ViewContext.ViewData["SavedMessage"];
      output.TagName = "span";
      output.Content.SetContent("Test...");
      base.Process(context, output);
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Bagombo.Models.ViewModels.Author
{
  public class EditBlogPostViewModel
  {
    [Required]
    public long Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    [DisplayFormat(DataFormatString = "{0:MMMM dd yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
    public DateTime PublishOn { get; set; }
    [Required]
    public bool Public { get; set; }
    //[Required]
    public List<TopicsCheckBox> TopicsList { get; set; }
    //[Required]
    public List<CategoriesCheckBox> CategoriesList { get; set; }
  }

  public class CategoriesCheckBox
  {
    public long CategoryId { get; set; }
    public string Name { get; set; }
    public bool IsSelected { get; set; }
  }

  public class TopicsCheckBox
  {
    public long TopicId { get; set; }
    public string Title { get; set; }
    public bool IsSelected { get; set; }
  }
}

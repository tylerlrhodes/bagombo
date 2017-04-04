using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bagombo.Models.ViewModels.Admin
{
  public class AdminManageCategoriesViewModel
  {
    public IEnumerable<Category> Categories { get; set; }
  }
}

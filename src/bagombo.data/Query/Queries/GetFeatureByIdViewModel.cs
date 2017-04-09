using Bagombo.Models.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Query.Queries
{
  public class GetFeatureByIdViewModel : IQuery<FeatureViewModel>
  {
    public long Id { get; set; }
  }
}

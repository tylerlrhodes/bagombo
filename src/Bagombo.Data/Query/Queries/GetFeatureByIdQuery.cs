using Bagombo.Models;
using Bagombo.Models.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Query.Queries
{
  public class GetFeatureByIdQuery : IQuery<Feature>
  {
    public long Id { get; set; }
  }
}

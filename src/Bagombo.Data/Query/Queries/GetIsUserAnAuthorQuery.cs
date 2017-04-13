using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Query.Queries
{
  public class GetIsUserAnAuthorQuery : IQuery<bool>
  {
    public string Id { get; set; }
  }
}

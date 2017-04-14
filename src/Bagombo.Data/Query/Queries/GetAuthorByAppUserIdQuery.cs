using Bagombo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Query.Queries
{
  public class GetAuthorByAppUserIdQuery : IQuery<Author>
  {
    public string Id { get; set; }
  }
}

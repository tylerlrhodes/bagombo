using Bagombo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Query.Queries
{
  public class GetAuthorByIdQuery : IQuery<Author>
  {
    public long Id { get; set; }
  }
}

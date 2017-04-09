using Bagombo.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Query.EFCoreQueryHandlers
{
  public abstract class EFQHBase
  {
    protected BlogDbContext _context;

    public EFQHBase(BlogDbContext context)
    {
      _context = context;
    }
  }
}

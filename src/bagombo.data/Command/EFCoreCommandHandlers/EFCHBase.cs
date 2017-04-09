using Bagombo.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  public abstract class EFCHBase
  {
    protected BlogDbContext _context;

    public EFCHBase(BlogDbContext context)
    {
      _context = context;
    }
  }
}

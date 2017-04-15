using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Command.Commands
{
  public class UpdateAuthorCommand
  {
    public long Id { get; set; }
    public string NewFirstName { get; set; }
    public string NewLastName { get; set; }
  }
}

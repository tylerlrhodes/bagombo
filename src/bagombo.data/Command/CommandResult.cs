using System;
using System.Collections.Generic;
using System.Text;

namespace Bagombo.Data.Command
{
  public class CommandResult<TCommand>
  {
    public bool Succeeded { get; set; }
    public TCommand Command { get; set; }

    public CommandResult(TCommand command, bool succeeded)
    {
      Succeeded = succeeded;
      Command = command;
    }
  }
}

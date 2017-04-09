using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bagombo.Data.Command
{
  interface ICommandHandlerAsync<TCommand>
  {
    Task<CommandResult<TCommand>> ExecuteAsync(TCommand command);
  }
}

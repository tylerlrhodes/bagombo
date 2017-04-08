using System;
using System.Collections.Generic;
using System.Text;
using SimpleInjector;
using System.Threading.Tasks;

namespace Bagombo.Data.Command
{
  public class CommandProcessor : ICommandProcessor
  {
    private Container _provider;

    public CommandProcessor(Container provider)
    {
      _provider = provider;
    }

    public async Task<CommandResult<TCommand>> ProcessAsync<TCommand>(TCommand command)
    {
      var handlerType = typeof(ICommandHandlerAsync<>).MakeGenericType(command.GetType());

      dynamic handler = _provider.GetInstance(handlerType);

      return await handler.ExecuteAsync((dynamic)command);
    }
  }

}

using System;
using System.Collections.Generic;
using System.Text;
using SimpleInjector;
using Bagombo.Data.Command.Commands;

namespace Bagombo.Data.Command.EFCoreCommandHandlers
{
  public static class EFCommandExtensions
  {
    public static void AddEFCommands(this Container provider)
    {

      provider.Register<ICommandHandlerAsync<AddFeatureCommand>, AddFeatureCommandEFCommandHandler>();

      provider.Register<ICommandProcessor, CommandProcessor>(Lifestyle.Scoped);
    }
  }
}

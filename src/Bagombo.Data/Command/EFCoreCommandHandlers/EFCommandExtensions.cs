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
      provider.Register<ICommandHandlerAsync<AddBlogPostCommand>, AddBlogPostCommandEFCH>();
      provider.Register<ICommandHandlerAsync<UpdateAuthorCommand>, UpdateAuthorCommandEFCH>();
      provider.Register<ICommandHandlerAsync<SetAppUserIdNullForAuthorCommand>, SetAppUserIdNullForAuthorEFCH>();
      provider.Register<ICommandHandlerAsync<AddAuthorCommand>, AddAuthorCommandEFCH>();
      provider.Register<ICommandHandlerAsync<DeleteCategoryCommand>, DeleteCategoryCommandEFCH>();
      provider.Register<ICommandHandlerAsync<EditCategoryCommand>, EditCategoryCommandEFCH>();
      provider.Register<ICommandHandlerAsync<AddCategoryCommand>, AddCategoryCommandEFCH>();
      provider.Register<ICommandHandlerAsync<DeleteFeatureCommand>, DeleteFeatureCommandEFCH>();
      provider.Register<ICommandHandlerAsync<EditFeatureCommand>, EditFeatureCommandEFCH>();
      provider.Register<ICommandHandlerAsync<AddFeatureCommand>, AddFeatureCommandEFCommandHandler>();

      provider.Register<ICommandProcessorAsync, CommandProcessor>(Lifestyle.Scoped);
    }
  }
}

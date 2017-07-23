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
      provider.Register<ICommandHandlerAsync<DeleteBlogPostCommand>, DeleteBlogPostCommandEFCH>();
      provider.Register<ICommandHandlerAsync<SetBlogPostCategoriesCommand>, SetBlogPostCategoriesCommandEFCH>();
      provider.Register<ICommandHandlerAsync<SetBlogPostTopicsCommand>, SetBlogPostTopicsCommandEFCH>();
      provider.Register<ICommandHandlerAsync<UpdateBlogPostCommand>, UpdateBlogPostCommandEFCH>();
      provider.Register<ICommandHandlerAsync<AddBlogPostCommand>, AddBlogPostCommandEFCH>();
      provider.Register<ICommandHandlerAsync<UpdateAuthorCommand>, UpdateAuthorCommandEFCH>();
      provider.Register<ICommandHandlerAsync<SetAppUserIdNullForAuthorCommand>, SetAppUserIdNullForAuthorEFCH>();
      provider.Register<ICommandHandlerAsync<AddAuthorCommand>, AddAuthorCommandEFCH>();
      provider.Register<ICommandHandlerAsync<DeleteCategoryCommand>, DeleteCategoryCommandEFCH>();
      provider.Register<ICommandHandlerAsync<EditCategoryCommand>, EditCategoryCommandEFCH>();
      provider.Register<ICommandHandlerAsync<AddCategoryCommand>, AddCategoryCommandEFCH>();
      provider.Register<ICommandHandlerAsync<DeleteTopicCommand>, DeleteTopicCommandEFCH>();
      provider.Register<ICommandHandlerAsync<EditTopicCommand>, EditTopicCommandEFCH>();
      provider.Register<ICommandHandlerAsync<AddTopicCommand>, AddTopicCommandEFCH>();
      provider.Register<ICommandHandlerAsync<EditAuthorProfileCommand>, EditAuthorProfileCommandEFCH>();

      provider.Register<ICommandProcessorAsync, CommandProcessor>(Lifestyle.Scoped);
    }
  }
}

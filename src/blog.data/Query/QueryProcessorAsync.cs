using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace blog.data.Query
{
  public class QueryProcessorAsync : IQueryProcessorAsync
  {
    IServiceProvider _provider;

    public QueryProcessorAsync(IServiceProvider provider)
    {
      _provider = provider;
    }

    public async Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query)
    {
      var handlerType = typeof(IQueryHandlerAsync<,>).MakeGenericType(query.GetType(), typeof(TResult));

      dynamic handler = _provider.GetService(handlerType);

      return await handler.HandleAsync((dynamic)query);
    }
  }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using SimpleInjector;

namespace Bagombo.Data.Query
{
  public class QueryProcessorAsync : IQueryProcessorAsync
  {
    Container _provider;

    public QueryProcessorAsync(Container provider)
    {
      _provider = provider;
    }

    public async Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query)
    {
      var handlerType = typeof(IQueryHandlerAsync<,>).MakeGenericType(query.GetType(), typeof(TResult));

      dynamic handler = _provider.GetInstance(handlerType);

      return await handler.HandleAsync((dynamic)query);
    }
  }
}

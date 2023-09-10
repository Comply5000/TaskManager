using System.Transactions;
using MediatR;
using TaskManager.Core.Common.Requests;

namespace TaskManager.Infrastructure.EF.Common.Decorators;

public class TransactionDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is ITransactionalRequest)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var response = await next();

                scope.Complete();

                return response;
            }
            catch
            {
                // Transaction will be automatically rolled back
                throw;
            }
            finally
            {
                scope.Dispose();
            }
        }
        else
        {
            // If the request does not implement the ITransactionalRequest interface, skip the transaction and proceed to the next handler
            return await next();
        }
    }
}
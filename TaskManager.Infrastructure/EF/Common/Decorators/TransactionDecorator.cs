using System.Transactions;
using MediatR;
using TaskManager.Core.Common.Requests;
using TaskManager.Infrastructure.EF.Context;

namespace TaskManager.Infrastructure.EF.Common.Decorators;

public class TransactionDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly EFContext _context;

    public TransactionDecorator(EFContext context)
    {
        _context = context;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is ITransactionalRequest)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var response = await next();

                await _context.SaveChangesAsync(cancellationToken);

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
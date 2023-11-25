using System.Transactions;
using MediatR;
using TaskManager.Infrastructure.EF.Context;
using TaskManager.Shared.Requests;

namespace TaskManager.Infrastructure.EF.Common.PipelineBehaviors;

public sealed class TransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : notnull
{
    private readonly EFContext _context;

    public TransactionPipelineBehavior(EFContext context)
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
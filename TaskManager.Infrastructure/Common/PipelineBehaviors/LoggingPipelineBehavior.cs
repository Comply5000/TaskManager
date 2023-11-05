// using MediatR;
// using Microsoft.Extensions.Logging;
// using Serilog;
// using TaskManager.Core.Shared.Services;
//
// namespace TaskManager.Infrastructure.EF.Common.PipelineBehaviors;
//
// public sealed class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//     where TRequest : notnull
// {
//     private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;
//     private readonly IDateService _dateService;
//
//     public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger, IDateService dateService)
//     {
//         _logger = logger;
//         _dateService = dateService;
//     }
//     
//     public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
//     {
//         var result = await next();
//         
//         _logger.LogWarning($"Completed request {typeof(TRequest).Name}, {_dateService.CurrentDate()}");
//
//         return result;
//     }
// }
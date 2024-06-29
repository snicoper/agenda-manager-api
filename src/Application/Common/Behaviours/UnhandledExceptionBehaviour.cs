using MediatR;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Application.Common.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse>(ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            string requestName = typeof(TRequest).Name;

            logger.LogError(
                ex,
                "EmployeeManager Request: Unhandled Exception for Request {Name} {@Request}",
                requestName,
                request);

            throw;
        }
    }
}

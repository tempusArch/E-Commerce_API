using System.Diagnostics;
using MediatR;
using Serilog;

namespace ECommerceAPI.Application;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> {
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;
    private readonly ICurrentUserAccessor _userAccessor;
    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger, ICurrentUserAccessor userAccessor) {
        _logger = logger;
        _userAccessor = userAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
        var requestName = typeof(TRequest).FullName;
        var userId = _userAccessor.GetCurrentUserId();
        var user = userId == 0 ? "Anonymous User" : userId.ToString();

        var _stopWatch = Stopwatch.StartNew();

        _logger.LogInformation("Request Started: {requestName}, by user: {user}, at {time}",
            requestName,
            user,
            DateTime.UtcNow
        );

        try {
            var response = await next();

            _stopWatch.Stop();

            _logger.LogInformation("Request Completed: {requestName}, by user: {user}, took {elapsed}ms, at {time}",
                requestName,
                user,
                _stopWatch.ElapsedMilliseconds,
                DateTime.UtcNow
            );

            return response;
        } catch (Exception ex) {
            _stopWatch.Stop();

            _logger.LogError(ex, "Request Failed: {requestName}, by user: {user}, took {elapsed}ms, at {time}",
                requestName,
                user,
                _stopWatch.ElapsedMilliseconds,
                DateTime.UtcNow
            );

            throw;

        }

    }
}
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours
{
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
        
        private static readonly Action<ILogger, string, long, TRequest, Exception?> Log =
            LoggerMessage.Define<string, long, TRequest>(
                LogLevel.Information, 
                new EventId(1, nameof(RequestPreProcessor<TRequest>)), 
                "Request time: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}");

        public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
            _timer = new Stopwatch();
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, 
            RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();
            
            var requestName = typeof(TRequest).Name;
            var elapsedMilliseconds = _timer.ElapsedMilliseconds;
            Log(_logger, requestName, elapsedMilliseconds, request, null);

            return response;
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours
{
    public class RequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger<RequestPreProcessor<TRequest>> _logger;

        private static readonly Action<ILogger<RequestPreProcessor<TRequest>>, string, TRequest, Exception?> Log =
            LoggerMessage.Define<string, TRequest>(LogLevel.Information, 1, "Request: {Name} {@Request}");

        public RequestPreProcessor(ILogger<RequestPreProcessor<TRequest>> logger)
        {
            _logger = logger;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            Log(_logger, requestName, request, null);
            
            return Task.CompletedTask;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Polly;
using Polly.Retry;

namespace Infrastructure.Common
{
    public static class DapperExtensions
    {
        private static readonly IEnumerable<TimeSpan> RetryTimes = new[]
        {
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(2),
            TimeSpan.FromSeconds(3)
        };
        
        private static readonly AsyncRetryPolicy RetryPolicy = Policy
            .Handle<NpgsqlException>()
            .Or<TimeoutException>()
            .WaitAndRetryAsync(RetryTimes,
                (exception, timeSpan, retryCount, context) =>
                {
                    //TODO: log here
                });
        
        public static async Task<T> QuerySingleOrDefaultAsyncWithRetry<T>(this IDbConnection cnn, 
            string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, 
            CommandType? commandType = null) =>
            await RetryPolicy.ExecuteAsync(async () =>
            {
                return await cnn.QuerySingleOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
            });
    }
}
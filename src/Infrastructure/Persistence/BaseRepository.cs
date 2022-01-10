using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Persistence
{
    public abstract class BaseRepository
    {
        private string ConnectionString { get; init; }

        protected NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        protected BaseRepository(IConfiguration configuration, string connectionStringName)
        {
            ConnectionString = configuration.GetConnectionString(connectionStringName);
            Guard.Against.NullOrEmpty(ConnectionString, nameof(ConnectionString));
        }
    }
}
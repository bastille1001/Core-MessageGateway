using System;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence
{
    public class MessageStateRepository : BaseRepository, IMessageStateRepository
    {
        public MessageStateRepository(IConfiguration configuration) 
            : base(configuration, "DSC")
        {
        }

        public async Task<int> GetState(Guid id)
        {
            await using var db = GetConnection();

            const string query = "SELECT STATE FROM MESSAGESTATE_AVRO WHERE id = @id";

            return await db.QuerySingleOrDefaultAsyncWithRetry<int>(query, new {id});
        }
    }
}
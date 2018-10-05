using System;
using Respawn;
using Npgsql;

namespace HaruGaKita.Test
{
    public class IntegrationTest : IDisposable
    {
        public Checkpoint _checkpoint { get; }
        readonly string _connectionString;

        public IntegrationTest(string ConnectionString)
        {
            _connectionString = ConnectionString;
            _checkpoint = new Checkpoint
            {
                SchemasToInclude = new[] { "public" },
                DbAdapter = DbAdapter.Postgres
            };
        }

        public async void Dispose()
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                await _checkpoint.Reset(conn);
            }
        }
    }
}
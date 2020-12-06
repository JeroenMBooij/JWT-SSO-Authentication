using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationServer.Common.Interfaces.Domain;

namespace AuthenticationServer.Domain.DataAccess.DataContext
{
    public class MainSqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;


        public string Connectionstring { get; set; } = "MainConnection";

        public MainSqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public async Task<T> GetData<T, U>(string sql, U parameters)
        {
            string connectionString = _config.GetConnectionString(Connectionstring);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                IEnumerable<T> data = await connection.QueryAsync<T>(sql, parameters);

                return data.FirstOrDefault();
            }
        }

        public async Task<List<T>> GetAllData<T, U>(string sql, U parameters)
        {
            string connectionString = _config.GetConnectionString(Connectionstring);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                IEnumerable<T> data = await connection.QueryAsync<T>(sql, parameters);

                return data.ToList();
            }
        }

        public async Task SaveData<T>(string sql, T parameters)
        {
            string connectionString = _config.GetConnectionString(Connectionstring);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}

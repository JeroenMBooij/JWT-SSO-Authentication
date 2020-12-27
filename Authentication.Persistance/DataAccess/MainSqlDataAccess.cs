using AuthenticationServer.Common.Interfaces.Domain.DataAccess;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Domain.DataAccess.DataContext
{
    public class MainSqlDataAccess : IMainSqlDataAccess
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

        public async Task ExecuteStoredProcedures<T>(Dictionary<T, string> ParametersToStoredProcedure)
        {
            string connectionString = _config.GetConnectionString(Connectionstring);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach ((T parameters, string storedProcedureName) in ParametersToStoredProcedure)
                            await connection.QueryAsync(storedProcedureName, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"saving your changes in the database failed: {ex.Message}");
                    }
                }

            }
        }

        public async Task SaveData<T>(string sql, T parameters)
        {
            string connectionString = _config.GetConnectionString(Connectionstring);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync(sql, parameters);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"saving your changes in the database failed: {ex.Message}");
                    }
                }

            }
        }
    }
}

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
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        private string _connectionString;
        private readonly IConfiguration _config;



        public MainSqlDataAccess(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("MainIdentityConnection");
        }


        public async Task<T> GetData<T, U>(string sql, U parameters)
        {
            var data = await GetAllData<T, U>(sql, parameters);

            return data.FirstOrDefault();
        }

        public async Task<TReturn> GetData<TFirst, TSecond, TReturn, U>(string sql, U parameters, Func<TFirst, TSecond, TReturn> mapper)
        {
            var data = await GetAllData<TFirst, TSecond, TReturn, U>(sql, parameters, mapper);

            return data.FirstOrDefault();
        }

        public async Task<TReturn> GetData<TFirst, TSecond, TThird, TReturn, U>(string sql, U parameters, Func<TFirst, TSecond, TThird, TReturn> mapper)
        {
            var data = await GetAllData<TFirst, TSecond, TThird, TReturn, U>(sql, parameters, mapper);

            return data.FirstOrDefault();
        }

        public async Task<TReturn> GetData<TFirst, TSecond, TThird, TFourth, TReturn, U>(string sql, U parameters, Func<TFirst, TSecond, TThird, TFourth, TReturn> mapper)
        {
            var data = await GetAllData<TFirst, TSecond, TThird, TFourth, TReturn, U>(sql, parameters, mapper);

            return data.FirstOrDefault();
        }

        public async Task<List<T>> GetAllData<T, U>(string sql, U parameters)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                IEnumerable<T> data = await connection.QueryAsync<T>(sql, parameters);

                return data.ToList();
            }
        }

        public async Task<List<TReturn>> GetAllData<TFirst, TSecond, TReturn, U>(string sql, U parameters, 
            Func<TFirst, TSecond, TReturn> mapper)
        {
            if(_connection is not null && _transaction is not null)
            {
                IEnumerable<TReturn> data = await _connection.QueryAsync(sql, mapper, parameters, _transaction);

                return data.ToList();
            }   
            
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                IEnumerable<TReturn> data = await connection.QueryAsync(sql, mapper, parameters);

                return data.ToList();
            }
        }

        public async Task<List<TReturn>> GetAllData<TFirst, TSecond, TThird, TReturn, U>(string sql, U parameters,
            Func<TFirst, TSecond, TThird, TReturn> mapper)
        {
            if (_connection is not null && _transaction is not null)
            {
                IEnumerable<TReturn> data = await _connection.QueryAsync(sql, mapper, parameters, _transaction);

                return data.ToList();
            }

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                IEnumerable<TReturn> data = await connection.QueryAsync(sql, mapper, parameters);

                return data.ToList();
            }
        }


        public async Task<List<TReturn>> GetAllData<TFirst, TSecond, TThird, TFourth, TReturn, U>(string sql, U parameters, 
            Func<TFirst, TSecond, TThird, TFourth, TReturn> mapper)
        {
            if (_connection is not null && _transaction is not null)
            {
                IEnumerable<TReturn> data = await _connection.QueryAsync(sql, mapper, parameters, _transaction);

                return data.ToList();
            }

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                IEnumerable<TReturn> data = await connection.QueryAsync(sql, mapper, parameters);

                return data.ToList();
            }
        }


        public async Task ExecuteStoredProcedures<T>(Dictionary<T, string> ParametersToStoredProcedure)
        {
            if (_connection is not null && _transaction is not null)
                await ExecuteStoredProceduresInTransaction(ParametersToStoredProcedure);
            else
                await ExecuteStoredProceduresInternal(ParametersToStoredProcedure);
        }

        private async Task ExecuteStoredProceduresInTransaction<T>(Dictionary<T, string> ParametersToStoredProcedure)
        {
            try
            {
                foreach ((T parameters, string storedProcedureName) in ParametersToStoredProcedure)
                    await _connection.QueryAsync(storedProcedureName, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction);
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
        }

        private async Task ExecuteStoredProceduresInternal<T>(Dictionary<T, string> ParametersToStoredProcedure)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
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
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

            }
        }

        public async Task SaveData<U>(string sql, U parameters)
        {
            if (_connection is not null && _transaction is not null)
                await SaveDataInTransaction(sql, parameters);
            else
                await SaveDataInternal(sql, parameters);
        }

        private async Task SaveDataInternal<U>(string sql, U parameters)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync(sql, parameters, transaction);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

            }
        }

        private async Task SaveDataInTransaction<U>(string sql, U parameters)
        {
            try
            {
                await _connection.ExecuteAsync(sql, parameters, _transaction);
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
        }

        public void BeginTransaction()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();

            _transaction = _connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();

            _connection = null;
            _transaction = null;
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();

            _connection = null;
            _transaction = null;
        }

        public void Dispose()
        {
            CommitTransaction();
        }

    }

}

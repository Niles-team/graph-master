using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace graph_master.data.dao
{
    public abstract class BaseDao
    {
        protected readonly string connectionString;
        protected BaseDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected IEnumerable<T> Query<T>(string sql, object parameters = null)
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    return connection.Query<T>(sql, parameters);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    throw exc;
                }
            }
        }
        protected T QueryFirst<T>(string sql, object parameters = null)
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    return connection.QueryFirst<T>(sql, parameters);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc);
                    throw exc;
                }
            }
        }
        protected T QueryFirstOrDefault<T>(string sql, object parameters = null)
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    return connection.QueryFirstOrDefault<T>(sql, parameters);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc);
                    throw exc;
                }
            }
        }
        protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    return await connection.QueryAsync<T>(sql, parameters);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    throw exc;
                }
            }
        }
        protected async Task<T> QueryFirstAsync<T>(string sql, object parameters = null)
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    return await connection.QueryFirstAsync<T>(sql, parameters);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc);
                    throw exc;
                }
            }
        }
        protected async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null)
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc);
                    throw exc;
                }
            }
        }
        protected T QuerySingle<T>(string sql, object parameters = null)
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    return connection.QuerySingle<T>(sql, parameters);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc);
                    throw exc;
                }
            }
        }
        protected T QuerySingleOrDefault<T>(string sql, object parameters = null)
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    return connection.QuerySingleOrDefault<T>(sql, parameters);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc);
                    throw exc;
                }
            }
        }
        protected async Task<T> QuerySingleAsync<T>(string sql, object parameters = null)
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    return await connection.QuerySingleAsync<T>(sql, parameters);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc);
                    throw exc;
                }
            }
        }
        protected async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object parameters = null)
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    return await connection.QuerySingleOrDefaultAsync<T>(sql, parameters);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc);
                    throw exc;
                }
            }
        }
        
        protected void Execute(string sql, object parameters = null)
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    connection.Execute(sql, parameters);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    throw exc;
                }
            }
        }
        protected async Task ExecuteAsync(string sql, object parameters = null)
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    await connection.ExecuteAsync(sql, parameters);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    throw exc;
                }
            }
        }

        protected int? Insert<TEntity>(TEntity entity, IDbTransaction transaction = null, int? commandTimeout = null) where TEntity : class
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    return (int?)connection.Insert(entity, transaction, commandTimeout);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    throw exc;
                }
            }
        }
        protected async Task<int?> InsertAsync<TEntity>(TEntity entity, IDbTransaction transaction = null, int? commandTimeout = null) where TEntity : class
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    return (int?)(await connection.InsertAsync(entity, transaction, commandTimeout));
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    throw exc;
                }
            }
        }
        protected bool Update<TEntity>(TEntity entity, IDbTransaction transaction = null, int? commandTimeout = null) where TEntity : class
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    return connection.Update(entity, transaction, commandTimeout);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    throw exc;
                }
            }
        }
        protected async Task<bool> UpdateAsync<TEntity>(TEntity entity, IDbTransaction transaction = null, int? commandTimeout = null) where TEntity : class
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    return await connection.UpdateAsync(entity, transaction, commandTimeout);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    throw exc;
                }
            }
        }
        protected bool Delete<TEntity>(TEntity entity, IDbTransaction transaction = null, int? commandTimeout = null) where TEntity : class
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    return connection.Delete(entity, transaction, commandTimeout);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    throw exc;
                }
            }
        }
        protected bool DeleteAll<TEntity>(IDbTransaction transaction = null, int? commandTimeout = null) where TEntity : class
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    return connection.DeleteAll<TEntity>(transaction, commandTimeout);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    throw exc;
                }
            }
        }
        protected async Task<bool> DeleteAsync<TEntity>(TEntity entity, IDbTransaction transaction = null, int? commandTimeout = null) where TEntity : class
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    return await connection.DeleteAsync(entity, transaction, commandTimeout);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    throw exc;
                }
            }
        }
        protected async Task<bool> DeleteAllAsync<TEntity>(IDbTransaction transaction = null, int? commandTimeout = null) where TEntity : class
        {
            using(IDbConnection connection = new Npgsql.NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    return await connection.DeleteAllAsync<TEntity>(transaction, commandTimeout);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    throw exc;
                }
            }
        }
    }
}
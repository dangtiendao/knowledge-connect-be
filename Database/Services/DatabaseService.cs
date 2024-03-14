using Dapper;
using KnowledgeConnect.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Services
{
    public class DatabaseService : IDatabaseService
    {
        /// <summary>
        /// GetByID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        public virtual async Task<object> GetByIDAsync(Type modelType, string id)
        {
            string commandText = string.Empty;
            var dicParam = new Dictionary<string, object>();
            commandText = GenerateSelectByID(modelType, dicParam, id);
            var data = await QueryUsingCommandText(commandText, dicParam);
            return data.FirstOrDefault();
        }

        /// <summary>
        /// GetAll
        /// </summary>
        /// <param name="modelType"></param>
        /// <returns></returns>
        public async Task<object> GetAllAsync(Type modelType)
        {
            string commandText = string.Empty;
            var dicParam = new Dictionary<string, object>();
            commandText = GenerateSelectAll(modelType, dicParam);
            var data = await QueryUsingCommandText(commandText, dicParam);
            return data;
        }

        /// <summary>
        /// Thực hiện Query commandText
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="dicParam"></param>
        /// <param name="transaction"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<object>> QueryUsingCommandText(string commandText, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            var cd = new CommandDefinition();
            try
            {
                List<object> result;
                var con = (transaction != null ? transaction.Connection : connection);
                if (con != null)
                {
                    cd = await BuildCommandDefinition(commandText, CommandType.Text, dicParam, transaction, connection);
                    result = (await con.QueryAsync(cd)).ToList();
                }
                else
                {
                    IDbConnection cnn = null;
                    try
                    {
                        cnn = GetConnection();
                        cd = await BuildCommandDefinition(commandText, CommandType.Text, dicParam, transaction, connection);
                        result = (await cnn.QueryAsync(cd)).ToList();
                    }
                    finally
                    {
                        cnn?.Dispose();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            throw new NotImplementedException();

        }

        /// <summary>
        /// Thực hiện query sử dụng stored
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dicParam"></param>
        /// <param name="transaction"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<object>> QueryUsingStoredProcedure(string commandText, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            var cd = new CommandDefinition();
            try
            {
                List<object> result;
                var con = (transaction != null ? transaction.Connection : connection);
                if (con != null)
                {
                    cd = await BuildCommandDefinition(commandText, CommandType.StoredProcedure, dicParam, transaction, connection);
                    result = (await con.QueryAsync(cd)).ToList();
                }
                else
                {
                    IDbConnection cnn = null;
                    try
                    {
                        cnn = GetConnection();
                        cd = await BuildCommandDefinition(commandText, CommandType.StoredProcedure, dicParam, transaction, connection);
                        result = (await cnn.QueryAsync(cd)).ToList();
                    }
                    finally
                    {
                        cnn?.Dispose();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            throw new NotImplementedException();

        }

        /// <summary>
        /// Get connection
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private IDbConnection? GetConnection()
        {
            var cnn = new MySqlConnection(DataBaseContext.ConnectionString ?? string.Empty);
            return cnn;
            throw new NotImplementedException();
        }

        private async Task<CommandDefinition> BuildCommandDefinition(string sql, CommandType commandType, Dictionary<string, object> dicParam, IDbTransaction? transaction, IDbConnection connection)
        {
            if (commandType == CommandType.StoredProcedure)
            {
                ///
            }
            else
            {
                ///
            }
            var commandDefinition = new CommandDefinition(commandText: sql, parameters: dicParam, transaction: transaction, commandType: commandType);
            return commandDefinition;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gen câu select by ID
        /// </summary>
        /// <param name="dicParam"></param>
        /// <param name="modelType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string GenerateSelectByID(Type modelType, Dictionary<string, object> dicParam, string id)
        {
            var instance = (BaseModel)Activator.CreateInstance(modelType);
            var tableName = instance.GetTableName();
            var key = instance.GetKeyProperty();
            dicParam.Add($"@{key}", id);
            string query = $"SELECT * FROM {tableName} WHERE {key} = @{key}";
            return query;
        }

        /// <summary>
        /// Gen câu get all
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="dicParam"></param>
        /// <returns></returns>
        private string GenerateSelectAll(Type modelType, Dictionary<string, object> dicParam = null)
        {
            var instance = (BaseModel)Activator.CreateInstance(modelType);
            var tableName = instance.GetTableName();
            string query = $"SELECT * FROM {tableName}";
            return query;
        }

    
    }
}

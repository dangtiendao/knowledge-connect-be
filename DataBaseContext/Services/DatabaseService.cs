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
    internal class DatabaseService
    {
        public async Task<T> GetByID<T>(string id, Type modelType) where T : BaseModel
        {
            string commandText = string.Empty;
            var dicParam = new Dictionary<string, object>();
            commandText = GenerateSelectByID(dicParam, modelType);
            var data = await QueryUsingCommandText<T>(commandText, dicParam);
            return data.FirstOrDefault();
        }

        private async Task<List<T>> QueryUsingCommandText<T>(string commandText, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null) where T : BaseModel
        {
            var cd = new CommandDefinition();
            try
            {
                List<T> result;
                var con = (transaction != null ? transaction.Connection : connection);
                if (con != null)
                {
                    cd = await BuildCommandDefinition(commandText, CommandType.Text, dicParam, transaction, connection);
                    result = (await con.QueryAsync<T>(cd)).ToList();
                }
                else
                {
                    IDbConnection cnn = null;
                    try
                    {
                        cnn = GetConnection();
                        cd = await BuildCommandDefinition(commandText, CommandType.Text, dicParam, transaction, connection);
                        result = (await con.QueryAsync<T>(cd)).ToList();
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

        private string GenerateSelectByID(Dictionary<string, object> dicParam, Type modelType)
        {
            throw new NotImplementedException();
        }
    }
}

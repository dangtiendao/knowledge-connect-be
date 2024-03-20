using Dapper;
using KnowledgeConnect.Common;
using KnowledgeConnect.Common.Constant;
using KnowledgeConnect.Common.Utilities;
using KnowledgeConnect.Model;
using MySqlConnector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        #region Get data

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
            var data = await QueryUsingCommandText(modelType, commandText, dicParam);
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
            var data = await QueryUsingCommandText(modelType, commandText, dicParam);
            return data;
        }

        /// <summary>
        /// Get dataPaging command text
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="pagingRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagingResponse> GetPagingAsync(Type modelType, PagingRequest pagingRequest)
        {
            var instance = (BaseModel)Activator.CreateInstance(modelType);
            //Generate table
            var tableName = instance.GetTableName();
            //Generate column
            var columns = pagingRequest.Columns ?? "*";
            //Generate where
            var whereStr = GenerateWhereString(pagingRequest.Filter);
            //Generate sort
            var sortStr = GenerateSortString(pagingRequest);
            //Generate paging
            var pagingStr = GeneratePagingString(pagingRequest);

            var cmdTextData = $"SELECT {columns} FROM {tableName} {whereStr} {sortStr} {pagingStr};";
            var cmdTextTotal = $"SELECT {columns} FROM {tableName} {whereStr};";
            var cmdText = $"{cmdTextData} {cmdTextTotal}";
            List<Type> types = new List<Type>() { modelType, typeof(Int32) };
            var res = await QueryMultiUsingCommandText(types, cmdText, null);
            return new PagingResponse
            {
                PageData = res.ElementAt(0),
                Total = (int)(res.ElementAt(1).FirstOrDefault()),
            };
        }

        #endregion

        #region Query sử dụng command text

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

        public async Task<List<object>> QueryUsingCommandText(Type modelType, string commandText, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            var cd = new CommandDefinition();
            try
            {
                List<object> result;
                var con = (transaction != null ? transaction.Connection : connection);
                if (con != null)
                {
                    cd = await BuildCommandDefinition(commandText, CommandType.Text, dicParam, transaction, connection);
                    result = (await con.QueryAsync(modelType, cd)).ToList();
                }
                else
                {
                    IDbConnection cnn = null;
                    try
                    {
                        cnn = GetConnection();
                        cd = await BuildCommandDefinition(commandText, CommandType.Text, dicParam, transaction, connection);
                        result = (await cnn.QueryAsync(modelType, cd)).ToList();
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

        public async Task<List<List<object>>> QueryMultiUsingCommandText(string commandText, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            var cd = new CommandDefinition();
            try
            {
                List<List<object>> result = new List<List<object>>();
                var con = (transaction != null ? transaction.Connection : connection);
                if (con != null)
                {
                    cd = await BuildCommandDefinition(commandText, CommandType.Text, dicParam, transaction, connection);
                    using (var multi = await con.QueryMultipleAsync(cd))
                    {
                        do
                        {
                            result.Add((await multi.ReadAsync()).ToList());

                        } while (!multi.IsConsumed);
                    }
                }
                else
                {
                    IDbConnection cnn = null;
                    try
                    {
                        cnn = GetConnection();
                        cd = await BuildCommandDefinition(commandText, CommandType.Text, dicParam, transaction, connection);
                        using (var multi = await cnn.QueryMultipleAsync(cd))
                        {
                            do
                            {
                                result.Add((await multi.ReadAsync()).ToList());

                            } while (!multi.IsConsumed);
                        }
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

        public async Task<List<List<object>>> QueryMultiUsingCommandText(List<Type> types, string commandText, Dictionary<string, object> dicParam = null, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            var cd = new CommandDefinition();
            try
            {
                List<List<object>> result = new List<List<object>>();
                var con = (transaction != null ? transaction.Connection : connection);
                if (con != null)
                {
                    cd = await BuildCommandDefinition(commandText, CommandType.Text, dicParam, transaction, connection);
                    using (var multi = await con.QueryMultipleAsync(cd))
                    {
                        foreach (Type t in types)
                        {
                            result.Add((await multi.ReadAsync(t)).ToList());
                        }
                    }
                }
                else
                {
                    IDbConnection cnn = null;
                    try
                    {
                        cnn = GetConnection();
                        cd = await BuildCommandDefinition(commandText, CommandType.Text, dicParam, transaction, connection);
                        using (var multi = await cnn.QueryMultipleAsync(cd))
                        {
                            foreach (Type t in types)
                            {
                                result.Add((await multi.ReadAsync(t)).ToList());
                            }
                        }
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

        #endregion

        #region Query sử dụng stored proceducre

        /// <summary>
        /// Thực hiện query sử dụng stored
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <param name="dicParam"></param>
        /// <param name="transaction"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<object>> QueryUsingStoredProcedure(string storedProcedure, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            var cd = new CommandDefinition();
            try
            {
                List<object> result;
                var con = (transaction != null ? transaction.Connection : connection);
                if (con != null)
                {
                    cd = await BuildCommandDefinition(storedProcedure, CommandType.StoredProcedure, dicParam, transaction, connection);
                    result = (await con.QueryAsync(cd)).ToList();
                }
                else
                {
                    IDbConnection cnn = null;
                    try
                    {
                        cnn = GetConnection();
                        cd = await BuildCommandDefinition(storedProcedure, CommandType.StoredProcedure, dicParam, transaction, connection);
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

        public async Task<List<object>> QueryUsingStoredProcedure(Type modelType, string storedProcedure, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            var cd = new CommandDefinition();
            try
            {
                List<object> result;
                var con = (transaction != null ? transaction.Connection : connection);
                if (con != null)
                {
                    cd = await BuildCommandDefinition(storedProcedure, CommandType.StoredProcedure, dicParam, transaction, connection);
                    result = (await con.QueryAsync(modelType, cd)).ToList();
                }
                else
                {
                    IDbConnection cnn = null;
                    try
                    {
                        cnn = GetConnection();
                        cd = await BuildCommandDefinition(storedProcedure, CommandType.StoredProcedure, dicParam, transaction, connection);
                        result = (await cnn.QueryAsync(modelType, cd)).ToList();
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
        
        public async Task<List<List<object>>> QueryMultiUsingStoredProcedure(string storedProcedure, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            var cd = new CommandDefinition();
            try
            {
                List<List<object>> result = new List<List<object>>();
                var con = (transaction != null ? transaction.Connection : connection);
                if (con != null)
                {
                    cd = await BuildCommandDefinition(storedProcedure, CommandType.StoredProcedure, dicParam, transaction, connection);
                    using (var multi = await con.QueryMultipleAsync(cd))
                    {
                        do
                        {
                            result.Add((await multi.ReadAsync()).ToList());

                        } while (!multi.IsConsumed);
                    }
                }
                else
                {
                    IDbConnection cnn = null;
                    try
                    {
                        cnn = GetConnection();
                        cd = await BuildCommandDefinition(storedProcedure, CommandType.StoredProcedure, dicParam, transaction, connection);
                        using (var multi = await cnn.QueryMultipleAsync(cd))
                        {
                            do
                            {
                                result.Add((await multi.ReadAsync()).ToList());

                            } while (!multi.IsConsumed);
                        }
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
        
        public async Task<List<List<object>>> QueryMultiUsingStoredProcedure(List<Type> types, string storedProcedure, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            var cd = new CommandDefinition();
            try
            {
                List<List<object>> result = new List<List<object>>();
                var con = (transaction != null ? transaction.Connection : connection);
                if (con != null)
                {
                    cd = await BuildCommandDefinition(storedProcedure, CommandType.StoredProcedure, dicParam, transaction, connection);
                    using (var multi = await con.QueryMultipleAsync(cd))
                    {
                        foreach (Type t in types)
                        {
                            result.Add((await multi.ReadAsync(t)).ToList());
                        }
                    }
                }
                else
                {
                    IDbConnection cnn = null;
                    try
                    {
                        cnn = GetConnection();
                        cd = await BuildCommandDefinition(storedProcedure, CommandType.StoredProcedure, dicParam, transaction, connection);
                        using (var multi = await cnn.QueryMultipleAsync(cd))
                        {
                            foreach (Type t in types)
                            {
                                result.Add((await multi.ReadAsync(t)).ToList());
                            }
                        }
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

        #endregion

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

        /// <summary>
        /// Xử lý Command
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="dicParam"></param>
        /// <param name="transaction"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private Task<CommandDefinition> BuildCommandDefinition(string sql, CommandType commandType, Dictionary<string, object> dicParam, IDbTransaction? transaction, IDbConnection connection)
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
            return Task.FromResult(commandDefinition);
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

        /// <summary>
        /// Thực hiện command text
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dicParam"></param>
        /// <param name="transaction"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> ExecuteScalarCommandText(string commandText, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            var cd = new CommandDefinition();
            try
            {
                bool result;
                var con = (transaction != null ? transaction.Connection : connection);
                if (con != null)
                {
                    cd = await BuildCommandDefinition(commandText, CommandType.Text, dicParam, transaction, connection);
                    result = (await con.ExecuteScalarAsync<int>(cd)) > 0;
                }
                else
                {
                    IDbConnection cnn = null;
                    try
                    {
                        cnn = GetConnection();
                        cd = await BuildCommandDefinition(commandText, CommandType.Text, dicParam, transaction, connection);
                        result = (await cnn.ExecuteScalarAsync<int>(cd)) > 0;

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
        /// Thực hiện store procedure
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <param name="dicParam"></param>
        /// <param name="transaction"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> ExecuteScalarStoredProcedure(string storedProcedure, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null)
        {
            var cd = new CommandDefinition();
            try
            {
                bool result;
                var con = (transaction != null ? transaction.Connection : connection);
                if (con != null)
                {
                    cd = await BuildCommandDefinition(storedProcedure, CommandType.StoredProcedure, dicParam, transaction, connection);
                    result = (await con.ExecuteScalarAsync<int>(cd)) > 0;
                }
                else
                {
                    IDbConnection cnn = null;
                    try
                    {
                        cnn = GetConnection();
                        cd = await BuildCommandDefinition(storedProcedure, CommandType.StoredProcedure, dicParam, transaction, connection);
                        result = (await cnn.ExecuteScalarAsync<int>(cd)) > 0;

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
        /// Paging
        /// </summary>
        /// <param name="pagingRequest"></param>
        /// <returns></returns>
        private string GeneratePagingString(PagingRequest pagingRequest)
        {
            var pagingStr = "";
            if (pagingRequest.PageSize == -1)
            {
                return pagingStr;
            }
            if (pagingRequest.PageSize > 0 && pagingRequest.PageIndex > 0)
            {
                var pageSize = pagingRequest.PageSize > PagingRequestConstants.MaxPage ? PagingRequestConstants.MaxPage : pagingRequest.PageSize;
                var pageIndex = pagingRequest.PageIndex;
                pagingStr = $"LIMMIT {pageSize} OFFSET {(pageIndex - 1) * pageSize} ";
            }
            return pagingStr;
        }

        /// <summary>
        /// Sort
        /// </summary>
        /// <param name="pagingRequest"></param>
        /// <returns></returns>
        private string GenerateSortString(PagingRequest pagingRequest)
        {
            if (string.IsNullOrEmpty(pagingRequest.Sort)) return string.Empty;
            var objectSorts = Utility.Deserialize<List<Dictionary<string, object>>>(pagingRequest.Sort);
            var sorts = new List<string>() { };
            foreach (var objectSort in objectSorts)
            {
                var objConvert = Utility.ConvertKeysToUpperCase(objectSort);
                var selector = objConvert.GetValue<string>("SELECTOR");
                var desc = objConvert.GetValue<bool>("DESC");
                var sortingAlgorithm = desc ? SortConstants.DESC : SortConstants.ASC;
                sorts.Add($" {selector} {sortingAlgorithm} ");
            }
            return $"ORDER BY {string.Join(",", sorts)} ";
        }

        /// <summary>
        /// Where
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        private string GenerateWhereString(string filter)
        {
            var whereStr = "";
            if (filter != null)
            {
                //whereStr = "WHERE ";
                whereStr = $"WHERE {ConvertFilterToWhere(filter)}";
            }
            return whereStr;
        }

        /// <summary>
        /// Filter => Where
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        private string ConvertFilterToWhere(string filter)
        {
            JArray filterArray = JsonConvert.DeserializeObject<JArray>(filter);

            StringBuilder whereClause = new StringBuilder();
            ParseFilterArray(filterArray, whereClause);

            return whereClause.ToString();
        }

        /// <summary>
        /// Convert fillter
        /// </summary>
        /// <param name="filterArray"></param>
        /// <param name="whereClause"></param>
        private void ParseFilterArray(JArray? filterArray, StringBuilder whereClause)
        {
            for (int i = 0; i < filterArray.Count; i++)
            {
                JToken item = filterArray[i];
                if (item is JArray)
                {
                    whereClause.Append("(");
                    ParseFilterArray((JArray)filterArray[i], whereClause);
                    whereClause.Append(")");
                }
                else if (item is JValue)
                {
                    JValue value = (JValue)item;
                    switch (i)
                    {
                        case 0:
                            //Column
                            whereClause.Append(value);
                            break;
                        case 1:
                            //Operator
                            whereClause.Append(GetComparisonOperator(value.ToString()));
                            break;
                        case 2:
                            //Value
                            //Xử lý các giá trị , trường hợp đặc biệt của value
                            var valueStr = value.ToString();
                            if (value.Type == JTokenType.Date)
                            {
                                valueStr = $"{((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss")}";
                            }
                            valueStr = HandleValueByOperator(filterArray[1].ToString(), valueStr);
                            whereClause.AppendFormat($"{valueStr}");
                            break;
                        default:
                            break;
                    }


                }
                whereClause.Append(" ");
            }
        }

        /// <summary>
        /// xử lý giá trị theo toán tử
        /// </summary>
        /// <param name="op"></param>
        /// <param name="valueStr"></param>
        /// <returns></returns>
        private string HandleValueByOperator(string op, string valueStr)
        {
            switch (op.ToUpper())
            {
                case Operator.Equal:
                case Operator.NotEqual:
                case Operator.GreaterThan:
                case Operator.GreaterOrEqual:
                case Operator.LessThan:
                case Operator.LessOrEqual:
                    return $"'{valueStr}'";

                case Operator.Contains:
                    return $"'%{valueStr}%'";
                case Operator.StartWith:
                    return $"'{valueStr}%'";
                case Operator.EndWith:
                    return $"'{valueStr}%'";
                case Operator.And:
                case Operator.Or:
                case Operator.Is:
                case Operator.IsNull:
                case Operator.NotNull:
                case Operator.IsNotNull:
                    return string.Empty;
                case Operator.In:
                case Operator.NotIn:
                    return $"({string.Join(",", valueStr.Split(";").Select(val => $"'{val}'").ToList())})";
                default: return $"'{valueStr}'";
            }
        }

        /// <summary>
        /// Toán tử
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        static string GetComparisonOperator(string op)
        {
            switch (op.ToUpper())
            {
                case Operator.Equal: return "=";
                case Operator.NotEqual: return "<>";
                case Operator.GreaterThan: return ">";
                case Operator.GreaterOrEqual: return ">=";
                case Operator.LessThan: return "<";
                case Operator.LessOrEqual: return "<=";
                case Operator.Contains:
                case Operator.StartWith:
                case Operator.EndWith:
                    return "LIKE";
                case Operator.And: return "AND";
                case Operator.Or: return "OR";
                case Operator.Is: return "IS";
                case Operator.IsNull: return "IS NULL";
                case Operator.NotNull: return "NOT NULL";
                case Operator.IsNotNull: return "IS NOT NULL";
                case Operator.In: return "IN";
                case Operator.NotIn: return "NOT IN";

                default: return op;
            }
        }

    }
}

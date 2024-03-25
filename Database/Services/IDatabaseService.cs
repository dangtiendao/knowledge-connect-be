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
    public interface IDatabaseService
    {
        public IDbConnection? GetConnection();

        #region Get
        public Task<object> GetByIDAsync(Type modelType, string id);
        public Task<object> GetAllAsync(Type modelType);
        public Task<PagingResponse> GetPagingAsync(Type modelType, PagingRequest pagingRequest);
        #endregion

        #region Query sử dụng command text
        public Task<List<object>> QueryAsyncUsingCommandText(string commandText, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);
        public Task<List<object>> QueryAsyncUsingCommandText(Type modelType, string commandText, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);

        public Task<List<List<object>>> QueryMultiAsyncUsingCommandText(string commandText, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);
        public Task<List<List<object>>> QueryMultiAsyncUsingCommandText(List<Type> types, string commandText, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);
        #endregion

        #region Query sử dụng stored proceducre

        public Task<List<object>> QueryAsyncUsingStoredProcedure(string storedProcedure, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);
        public Task<List<object>> QueryAsyncUsingStoredProcedure(Type modelType, string storedProcedure, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);

        public Task<List<List<object>>> QueryMultiAsyncUsingStoredProcedure(string storedProcedure, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);

        public Task<List<List<object>>> QueryMultiAsyncUsingStoredProcedure(List<Type> types, string storedProcedure, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);
        #endregion

        public Task<bool> ExecuteScalarAsyncUsingCommandText(string commandText, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);
        public Task<bool> ExecuteScalarAsyncUsingStoredProcedure(string storedProcedure, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);

    }
}

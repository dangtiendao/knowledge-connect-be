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
        #region Get
        public Task<object> GetByIDAsync(Type modelType, string id);
        public Task<object> GetAllAsync(Type modelType);
        public Task<PagingResponse> GetPagingAsync(Type modelType, PagingRequest pagingRequest);
        #endregion

        #region Query sử dụng command text
        public Task<List<object>> QueryUsingCommandText(string commandText, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);
        public Task<List<object>> QueryUsingCommandText(Type modelType, string commandText, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);

        public Task<List<List<object>>> QueryMultiUsingCommandText(string commandText, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);
        public Task<List<List<object>>> QueryMultiUsingCommandText(List<Type> types, string commandText, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);
        #endregion

        #region Query sử dụng stored proceducre

        public Task<List<object>> QueryUsingStoredProcedure(string storedProcedure, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);
        public Task<List<object>> QueryUsingStoredProcedure(Type modelType, string storedProcedure, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);

        public Task<List<List<object>>> QueryMultiUsingStoredProcedure(string storedProcedure, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);

        public Task<List<List<object>>> QueryMultiUsingStoredProcedure(List<Type> types, string storedProcedure, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);
        #endregion

        public Task<bool> ExecuteScalarCommandText(string commandText, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);
        public Task<bool> ExecuteScalarStoredProcedure(string storedProcedure, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);

    }
}

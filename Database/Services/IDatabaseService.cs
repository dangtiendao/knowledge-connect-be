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
        public Task<List<object>> QueryUsingCommandText(string commandText, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);
        public Task<List<object>> QueryUsingStoredProcedure(string storedProcedure, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);

        public Task<bool> ExecuteScalarCommandText(string commandText, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);
        public Task<bool> ExecuteScalarStoredProcedure(string storedProcedure, Dictionary<string, object> dicParam, IDbTransaction transaction = null, IDbConnection connection = null);

    }
}

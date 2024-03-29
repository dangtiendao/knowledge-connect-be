﻿using KnowledgeConnect.Common.Constant;
using KnowledgeConnect.Common.Utilities;
using KnowledgeConnect.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using KnowledgeConnect.Common;
using Database.Services;
using System.Data;
using System.Transactions;
using static Dapper.SqlMapper;
using KnowledgeConnect.Common.Enum;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;
using Microsoft.Extensions.Configuration;

namespace KnowledgeConnect.BL
{
    public class BaseBL : IBaseBL
    {
        private readonly IDatabaseService _databaseService;

        private IConfiguration _configuration;
        public BaseBL(IConfiguration configuration, IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _configuration = configuration;
        }

        public async Task<ServiceResponse> DeleteDataAsync(BaseModel baseModel)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> DeleteDatas(List<string> ids, Type currentModelType)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetAll(Type currentModelType)
        {
            throw new NotImplementedException();
        }

        public async Task<object> GetByIDAsync(Type currentModelType, string id)
        {
            return await _databaseService.GetByIDAsync(currentModelType, id);

            //TODO
            throw new NotImplementedException();
        }

        public async Task<PagingResponse> GetPagingAsync(Type currentModelType, PagingRequest pagingRequest)
        {
            var response = await _databaseService.GetPagingAsync(currentModelType, pagingRequest);
            return response;
            throw new NotImplementedException();
        }

        public Task<object> GetDetailByMasterID(Type currentModelType, string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lưu dữ liệu (có thể dùng chung thêm, sửa, xóa - do State model)
        /// </summary>
        /// <param name="baseModel"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ServiceResponse> SaveDataAsync(BaseModel baseModel)
        {
            var response = new ServiceResponse();
            //Validate
            var validateResults = await ValidateSaveData(baseModel);

            if (validateResults.Count > 0)
            {
                response.ValidateInfo = validateResults;
                response.Success = false;
                return response;
            }
            //BeforeSave
            await BeforSaveData(baseModel);

            //DoSave

            IDbConnection connection = _databaseService.GetConnection();
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();
            try
            {
                var result = await DoSaveAsync(baseModel, transaction);
                if (result)
                {
                    await AfterSaveAsync(baseModel, transaction);
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    response.Success = false;
                }
            }
            finally
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                }
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            if (response.Success)
            {
                AfterSaveCommit(baseModel);
            }

            return response;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sau khi commit
        /// </summary>
        /// <param name="baseModel"></param>
        public virtual async void AfterSaveCommit(BaseModel baseModel)
        {
        }

        /// <summary>
        /// Sau khi lưu
        /// </summary>
        /// <param name="baseModel"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task AfterSaveAsync(BaseModel baseModel, IDbTransaction transaction)
        {
        }

        /// <summary>
        /// Lưu model
        /// </summary>
        /// <param name="baseModel"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DoSaveAsync(BaseModel baseModel, IDbTransaction transaction)
        {
            baseModel.SetAutoPrimaryKey();
            var dic = new Dictionary<string, object>();
            var commandText = GetCommandTextByModelState(baseModel, dic);
            //Kiểu
            if (baseModel.GetPrimaryKeyType() == typeof(int) || baseModel.GetPrimaryKeyType() == typeof(long))
            {
                var primaryKey = (await _databaseService.QueryAsyncUsingCommandText(typeof(int), commandText, dic, transaction)).FirstOrDefault();
                if (baseModel.State == ModelState.Insert || baseModel.State == ModelState.Duplicate)
                {
                    baseModel.SetValueByAttribute(typeof(KeyAttribute), primaryKey);
                }
            }
            else
            {
                if (baseModel.State == ModelState.Insert || baseModel.State == ModelState.Duplicate)
                {
                    baseModel.SetAutoPrimaryKey();
                }
                await _databaseService.ExecuteScalarAsyncUsingCommandText(commandText, dic, transaction);
            }
            return true;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Command text theo state model
        /// </summary>
        /// <param name="baseModel"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string GetCommandTextByModelState(BaseModel baseModel, Dictionary<string, object> dic)
        {
            var commandText = string.Empty;
            switch (baseModel.State)
            {
                case ModelState.None:
                    break;
                case ModelState.Insert:
                case ModelState.Duplicate:
                    return BuildInsertCommandText(baseModel, dic);
                    break;
                case ModelState.Update:
                    return BuildUpdateCommandText(baseModel, dic);
                    break;
                case ModelState.Delete:
                    return BuildDeleteCommandText(baseModel, dic);
                    break;
                case ModelState.Restore:
                    break;
                case ModelState.Sync:
                    break;
                case ModelState.Merge:
                    break;
                default:
                    break;
            }
            return commandText;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Câu xóa model
        /// </summary>
        /// <param name="baseModel"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string BuildDeleteCommandText(BaseModel baseModel, Dictionary<string, object> dic)
        {
            //DELETE FROM table_name WHERE condition;
            //SELECT ROW_COUNT() AS rows_deleted;
            var stringBuilder = new StringBuilder();
            var tablename = baseModel.GetTableName();
            if (!string.IsNullOrEmpty(tablename))
            {
                stringBuilder.Append($"DELETE FROM {tablename} WHERE ");
                string key = baseModel.GetKeyProperty();
                var keyValue = baseModel.GetPrimaryKeyValue();
                stringBuilder.Append($"{key} = {keyValue.ToString()}");
                stringBuilder.Append("SELECT ROW_COUNT() AS rows_deleted;");

            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Câu update model
        /// </summary>
        /// <param name="baseModel"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string BuildUpdateCommandText(BaseModel baseModel, Dictionary<string, object> dic)
        {
            //UPDATE table_name
            //SET column1 = value1, column2 = value2, ...
            //WHERE condition;
            var stringBuilder = new StringBuilder();
            var tablename = baseModel.GetTableName();
            string key = baseModel.GetKeyProperty();
            var keyValue = baseModel.GetPrimaryKeyValue().ToString();
            if (!string.IsNullOrEmpty(tablename) && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(keyValue))
            {
                stringBuilder.Append($"UPDATE {tablename} SET ");

                var properties = this.GetType().GetProperties();
                var propertyInfos = properties.Where(property => property.GetCustomAttribute<NotMappedAttribute>() == null && property.GetCustomAttribute<KeyAttribute>() == null);
                List<string> values = new List<string>() { };
                foreach (var propertyInfo in propertyInfos)
                {
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        values.Add($"`{propertyInfo.Name}` = `{propertyInfo}`");
                    }
                    else if (propertyInfo.PropertyType == typeof(DateTime))
                    {
                        values.Add($"`{propertyInfo.Name}` = `{propertyInfo}`");
                    }
                    else if (propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(decimal))
                    {
                        values.Add($"`{propertyInfo.Name}` = {propertyInfo}");
                    }
                };
                stringBuilder.Append(string.Join(",", values));
                stringBuilder.Append($"WHERE {key} = {keyValue.ToString()}");
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Câu insert model
        /// </summary>
        /// <param name="baseModel"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string BuildInsertCommandText(BaseModel baseModel, Dictionary<string, object> dic)
        {
            //INSERT INTO table_name(column1, column2, column3, ...) VALUES(value1, value2, value3, ...);
            var stringBuilder = new StringBuilder();
            var tablename = baseModel.GetTableName();
            if (!string.IsNullOrEmpty(tablename))
            {
                stringBuilder.Append($"INSERT INTO {tablename} ");
                List<string> columns = new List<string>();
                var values = new List<object>();
                var propertyInfos = this.GetType().GetProperties().Where(propertyInfo => propertyInfo.GetCustomAttribute<NotMappedAttribute>() != null).ToList();
                foreach (var propertyInfo in propertyInfos)
                {
                    var value = propertyInfo.GetValue(baseModel);
                    if (value != null)
                    {
                        columns.Add(propertyInfo.Name);
                        values.Add(value);
                    }
                }
                stringBuilder.Append($"({string.Join(",", columns)})");
                stringBuilder.Append($"VALUES ({string.Join(",", values)});");
                stringBuilder.Append("SELECT LAST_INSERT_ID();");

            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Xử lý trước khi lưu
        /// </summary>
        /// <param name="baseModel"></param>
        /// <returns></returns>
        public virtual async Task BeforSaveData(BaseModel baseModel)
        {
            if (baseModel.State == ModelState.Insert || baseModel.State == ModelState.Duplicate)
            {
                baseModel.CreatedDate = DateTime.Now;
                //baseModel.CreatedBy = GetUser().FullName;
            }
            if (baseModel.State == ModelState.Update)
            {
                baseModel.ModifiedDate = DateTime.Now;
                //baseModel.ModifiedBy = GetUser().FullName;
            }
            //TODO
        }

        /// <summary>
        /// Validate trước khi lưu
        /// </summary>
        /// <param name="baseModel"></param>
        /// <returns></returns>
        public virtual async Task<List<ValidateResult>> ValidateSaveData(BaseModel baseModel)
        {
            var validateResults = new List<ValidateResult>() { };
            return validateResults;
        }

        public Task<ServiceResponse> SaveListDataAsync(List<BaseModel> baseModels)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> UpdateFieldAsync(FieldUpdate fieldUpdate)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> UpdateFieldsAsync(Type currentModelType, List<FieldUpdate> fieldUpdates)
        {
            throw new NotImplementedException();
        }

    }
}

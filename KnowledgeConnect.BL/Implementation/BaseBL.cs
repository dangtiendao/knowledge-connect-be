using KnowledgeConnect.Common.Constant;
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

namespace KnowledgeConnect.BL
{
    public class BaseBL : IBaseBL
    {
        private readonly IDatabaseService _databaseService;

        public BaseBL(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public Task<ServiceResponse> DeleteDataAsync(BaseModel baseModel)
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

        public async Task<object> GetPagingAsync(Type currentModelType, PagingRequest pagingRequest)
        {
            var response = await _databaseService.GetPagingAsync(currentModelType, pagingRequest);
            return response;
            throw new NotImplementedException();
        }

        public Task<object> GetDetailByMasterID(Type currentModelType, string id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> SaveDataAsync(BaseModel baseModel)
        {
            throw new NotImplementedException();
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

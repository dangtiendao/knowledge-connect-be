using KnowledgeConnect.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeConnect.BL
{
    public interface IBaseBL
    {
        Task<ServiceResponse> DeleteDataAsync(BaseModel baseModel);
        Task<ServiceResponse> DeleteDatas(List<string> ids, Type currentModelType);
        Task<object> GetAll(Type currentModelType);
        Task<object> GetByIDAsync(Type currentModelType, string id);
        Task<object> GetDataPagingAsync(Type currentModelType, PagingRequest pagingRequest);
        Task<object> GetDetailByMasterID(Type currentModelType, string id);
        Task<ServiceResponse> SaveDataAsync(BaseModel baseModel);
        Task<ServiceResponse> SaveListDataAsync(List<BaseModel> baseModels);
        Task<ServiceResponse> UpdateFieldAsync(FieldUpdate fieldUpdate);
        Task<ServiceResponse> UpdateFieldsAsync(Type currentModelType, List<FieldUpdate> fieldUpdates);
    }
}

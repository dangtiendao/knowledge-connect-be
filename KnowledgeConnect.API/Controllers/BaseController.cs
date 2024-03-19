using KnowledgeConnect.BL;
using KnowledgeConnect.Common.Enum;
using KnowledgeConnect.Common.Utilities;
using KnowledgeConnect.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections;
using System.Reflection;
using System.Text.Json.Serialization;

namespace KnowledgeConnect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        #region Declare
        private IBaseBL _baseBL;
        protected IBaseBL BaseBL
        {
            get
            {
                if (_baseBL == null)
                {
                    throw new NotImplementedException();
                }
                return _baseBL;
            }
            set { _baseBL = value; }
        }

        private Type _currentModelType;
        private IUserBL userBL;

        public BaseController(IBaseBL baseBL)
        {
            _baseBL = baseBL;
        }

        protected Type CurrentModelType
        {
            get
            {
                if (_currentModelType == null)
                {
                    throw new NotImplementedException();
                }
                return _currentModelType;
            }
            set
            {
                _currentModelType = value;
            }
        }
        #endregion

        #region HttpGet

        /// <summary>
        /// Get all
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<ServiceResponse> GetAll()
        {
            var res = new ServiceResponse();
            if (await ValidatePermission(res, Common.Enum.ModelState.None))
            {
                res.OnSuccess(await BaseBL.GetAll(this.CurrentModelType));
            }
            return res;
        }

        /// <summary>
        /// Lấy bản ghi theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual async Task<ServiceResponse> GetByID(string id)
        {
            var res = new ServiceResponse();
            if (await ValidatePermission(res, Common.Enum.ModelState.None))
            {
                res.OnSuccess(await BaseBL.GetByIDAsync(this.CurrentModelType, id));
            }
            return res;
        }

        /// <summary>
        /// Lấy master-detail theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("{id}/detail")]
        public virtual async Task<ServiceResponse> GetDetailByMasterID(string id, string filter = null)
        {
            var res = new ServiceResponse();
            if (await ValidatePermission(res, Common.Enum.ModelState.None))
            {
                res.OnSuccess(await BaseBL.GetDetailByMasterID(this.CurrentModelType, id));
            }
            return res;
        }
        #endregion

        #region HttpPost
        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="dataInsert"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<ServiceResponse> Insert([FromBody] object dataInsert)
        {
            var res = new ServiceResponse();
            var baseModel = (BaseModel)Utility.DeserializeObject(Utility.Serialize(dataInsert), this.CurrentModelType);
            baseModel.State = Common.Enum.ModelState.Insert;
            if (await ValidatePermission(res, Common.Enum.ModelState.Insert, baseModel))
            {
                res = await this.BaseBL.SaveDataAsync(baseModel);
            }
            return res;
        }

        /// <summary>
        /// Thêm mới nhiều bản ghi
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost("bluk")]
        public virtual async Task<ServiceResponse> SaveDatas([FromBody] IList models)
        {
            var res = new ServiceResponse();
            if (await ValidatePermission(res, Common.Enum.ModelState.Insert))
            {
                var baseModels = new List<BaseModel>();
                foreach(var model in models)
                {
                    var baseModel = (BaseModel)Utility.DeserializeObject(Utility.Serialize(model), this.CurrentModelType);
                    baseModel.State = Common.Enum.ModelState.Insert;
                    baseModels.Add(baseModel);
                }
                res = await this.BaseBL.SaveListDataAsync(baseModels);
            }
            return res;
        }

        /// <summary>
        /// Lấy danh sách bản ghi theo phân trang
        /// </summary>
        /// <param name="pagingRequest"></param>
        /// <returns></returns>
        [HttpPost("paging")]
        public virtual async Task<ServiceResponse> GetDataPaging([FromBody] PagingRequest pagingRequest)
        {
            var res = new ServiceResponse();
            if (await ValidatePermission(res, Common.Enum.ModelState.None))
            {
                res.OnSuccess(await this.BaseBL.GetPagingAsync(this.CurrentModelType, pagingRequest));
            }
            return res;
        }
        #endregion

        #region HttpPut
        /// <summary>
        /// Sửa bản ghi
        /// </summary>
        /// <param name="dataUpdate"></param>
        /// <returns></returns>
        [HttpPut]
        public virtual async Task<ServiceResponse> Update([FromBody] object dataUpdate)
        {
            var res = new ServiceResponse();
            var baseModel = (BaseModel)Utility.DeserializeObject(Utility.Serialize(dataUpdate), this.CurrentModelType);
            baseModel.State = Common.Enum.ModelState.Update;
            if (await ValidatePermission(res, Common.Enum.ModelState.Update, baseModel))
            {
                res = await this.BaseBL.SaveDataAsync(baseModel);
            }
            return res;
        }
        #endregion

        #region HttpPatch
        /// <summary>
        /// Cập nhật bản ghi
        /// </summary>
        /// <param name="fieldUpdate"></param>
        /// <returns></returns>
        [HttpPatch]
        public virtual async Task<ServiceResponse> UpdateField([FromBody] FieldUpdate fieldUpdate)
        {
            var res = new ServiceResponse();
            if (await ValidatePermission(res, Common.Enum.ModelState.Update))
            {
                res = await this.BaseBL.UpdateFieldAsync(fieldUpdate);
            }
            return res;
        }

        /// <summary>
        /// Cập nhật bản ghi
        /// </summary>
        /// <param name="fieldUpdates"></param>
        /// <returns></returns>
        [HttpPatch("bulk")]
        public virtual async Task<ServiceResponse> UpdateFields([FromBody] List<FieldUpdate> fieldUpdates)
        {
            var res = new ServiceResponse();
            if (await ValidatePermission(res, Common.Enum.ModelState.Update))
            {
                res = await this.BaseBL.UpdateFieldsAsync(this.CurrentModelType, fieldUpdates);
            }
            return res;
        }
        #endregion

        #region HttpDelete

        /// <summary>
        /// Xóa bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task<ServiceResponse> DeleteByID(string id)
        {
            var res = new ServiceResponse();
            var baseModel = (BaseModel)(await this.BaseBL.GetByIDAsync(this.CurrentModelType, id));
            if(baseModel == null)
            {
                res.OnError(userMessage: "Not Found");
                return res;
            }
            if (await ValidatePermission(res, Common.Enum.ModelState.Delete, baseModel))
            {
                baseModel.State = Common.Enum.ModelState.Delete;
                res = await this.BaseBL.DeleteDataAsync(baseModel);
                res = res.Success ? res.OnSuccess(baseModel) : res;
            }
            return res;
        }

        /// <summary>
        /// Xóa nhiều bản ghi theo id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public virtual async Task<ServiceResponse> Deletes([FromBody] List<string> ids)
        {
            var res = new ServiceResponse();
            if (await ValidatePermission(res, Common.Enum.ModelState.Delete))
            {
                res = await this.BaseBL.DeleteDatas(ids, this.CurrentModelType);
            }
            return res;
        }
        #endregion

        #region Check Permission

        /// <summary>
        /// Check quyền
        /// </summary>
        /// <param name="serviceResponse"></param>
        /// <param name="modelState"></param>
        /// <param name="baseModel"></param>
        /// <returns></returns>
        protected virtual async Task<bool> ValidatePermission(ServiceResponse serviceResponse, ModelState modelState, BaseModel baseModel = null)
        {
            return true;
        }
        #endregion
    }
}

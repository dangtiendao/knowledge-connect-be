using KnowledgeConnect.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeConnect.Model
{
    public class ServiceResponse
    {
        private List<ValidateResult> _validateInfo;
        public const string DEFAULT_ERRORMESSAGE = "Có lỗi trong quá trình xử lý";

        public List<ValidateResult> ValidateInfo
        {
            get
            {
                if( _validateInfo == null)
                {
                    this._validateInfo = new List<ValidateResult>();
                }
                return this._validateInfo;
            }
            set
            {
                this._validateInfo = value;
            }
        }
        /// <summary>
        /// Kết quả thực thi
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Mã lỗi chính
        /// </summary>
        public ServiceResponeCode Code { get; set; } = ServiceResponeCode.Success;

        /// <summary>
        /// Mã lỗi phụ
        /// </summary>
        public int SubCode { get; set; }

        /// <summary>
        /// Nội dung lỗi hiển thị người dùng
        /// </summary>
        public string UserMessage { get; set; }

        /// <summary>
        /// Nội dung lỗi của hệ thống
        /// </summary>
        public string SystemMessage { get; set; }

        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Giờ hiện tại server
        /// </summary>
        public DateTime ServerTime { get; set; } = DateTime.Now;

        #region Method

        /// <summary>
        /// Hàm tạo
        /// </summary>
        public ServiceResponse()
        {

        }

        /// <summary>
        /// Hàm tạo kèm data
        /// </summary>
        /// <param name="data"></param>
        public ServiceResponse(object data)
        {
            this.Data = data;
        }

        public override string ToString()
        {
            if (Success)
            {
                return "Success";
            }
            else
            {
                return $"Falied - code {Code}-{SubCode} - SystemMessage: {SystemMessage} - UserMessage: {UserMessage}";
            }
        }

        /// <summary>
        /// Gán giá trị trường hợp success
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ServiceResponse OnSuccess(object data = null)
        {
            this.Data = data;
            return this;
        }

        /// <summary>
        /// Gán giá trị khi gặp lỗi
        /// </summary>
        /// <param name="subCode"></param>
        /// <param name="data"></param>
        /// <param name="userMessage"></param>
        /// <param name="systemMessage"></param>
        /// <returns></returns>
        public ServiceResponse OnError(int subCode = 0, object data = null, string userMessage = DEFAULT_ERRORMESSAGE, string systemMessage = "")
        {
            this.Success = false;
            this.Code = ServiceResponeCode.Error;
            this.SubCode = subCode;
            this.SystemMessage = systemMessage;
            this.Data = data;
            this.SystemMessage = systemMessage;
            if (string.IsNullOrEmpty(systemMessage))
            {
                this.SystemMessage = $"{(int)(ServiceResponeCode.Error)}-{subCode}";
            }
            return this;
        }

        /// <summary>
        /// Gán giá trị khi gặp exception
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public ServiceResponse OnException(Exception ex)
        {
            if(ex != null)
            {
                this.Success = false;
                this.Code = ServiceResponeCode.Exception;
                this.UserMessage = DEFAULT_ERRORMESSAGE;
                this.SystemMessage = "Exception!";
            }

            return this;
        }

        #endregion
    }

    public class ErrorServiceResponse : ServiceResponse
    {
        public ErrorServiceResponse (int subCode = 0, object data = null, string userMessage = DEFAULT_ERRORMESSAGE, string systemMessage = "")
        {
            OnError(subCode, data, userMessage, systemMessage);
        }

    }

    public class ExceptionServiceResponse : ServiceResponse
    {
        public ExceptionServiceResponse(Exception ex)
        {
            OnException(ex);
        }

    }
}

using KnowledgeConnect.Model;
using KnowledgeConnect.Model.Authen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeConnect.BL
{
    public interface IAuthenBL: IBaseBL
    {

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        public Task<ServiceResponse> Login(LoginRequest loginRequest);

        /// <summary>
        /// Đăng xuất
        /// </summary>
        /// <returns></returns>
        public Task<ServiceResponse> Logout();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<ServiceResponse> ValidateToken(string token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Task<object> GetInfoFromToken(string? v);

    }
}

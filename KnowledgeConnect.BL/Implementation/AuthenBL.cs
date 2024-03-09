using KnowledgeConnect.Model;
using KnowledgeConnect.Model.Authen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeConnect.BL
{
    public class AuthenBL : BaseBL, IAuthenBL
    {

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ServiceResponse> Login(LoginRequest loginRequest)
        {
            var response = new ServiceResponse();
            if (String.IsNullOrEmpty(loginRequest.UserName) || String.IsNullOrEmpty(loginRequest.Password))
            {
                response.OnError();
            }
            else
            {

            }
            throw new NotImplementedException();
        }
    }
}

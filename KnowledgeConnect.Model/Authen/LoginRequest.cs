using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeConnect.Model.Authen
{
    public class LoginRequest
    {
        /// <summary>
        /// Tài khoản
        /// </summary>
        public string UserName { get; set; }
                     
        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string Password { get; set; }
    }
}

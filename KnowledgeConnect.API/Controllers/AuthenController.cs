using KnowledgeConnect.BL;
using KnowledgeConnect.Model;
using KnowledgeConnect.Model.Authen;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeConnect.API.Controllers
{
    public class AuthenController : BaseController
    {
        private readonly IAuthenBL _authenBL;
        public AuthenController (IAuthenBL authenBL)  :base(authenBL)
        {
            _authenBL = authenBL;
            this.CurrentModelType = typeof(User);
            this.BaseBL = (IBaseBL)authenBL;
        }

        [HttpPost("login")]
        public async Task<ServiceResponse> Login(LoginRequest loginRequest)
        {
            if(loginRequest == null)
            {
                return new ErrorServiceResponse(0,null,"","");
            }
            var res = await _authenBL.Login(loginRequest);
            return res;
        }
        
    }
}
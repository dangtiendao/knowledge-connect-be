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
        public AuthenController(IAuthenBL authenBL) : base(authenBL)
        {
            _authenBL = authenBL;
            this.CurrentModelType = typeof(User);
            this.BaseBL = (IBaseBL)authenBL;
        }

        [HttpPost("login")]
        public async Task<ServiceResponse> Login(LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return new ErrorServiceResponse(0, null, "", "");
            }
            var res = await _authenBL.Login(loginRequest);
            return res;
        }

        [HttpPost("logout")]
        public async Task<ServiceResponse> Logout()
        {
            var res = await _authenBL.Logout();
            return res;
        }

        [HttpPost("token-validation")]
        public async Task<ServiceResponse> VaidateToken([FromBody] string token)
        {
            if (token == null)
            {
                return new ErrorServiceResponse(0, null, "", "");
            }
            var response = await _authenBL.ValidateToken(token);

            return response;
        }

        [HttpPost("token-info")]
        public async Task<ServiceResponse> GetInfoFromToken([FromBody] string token)
        {
            if (token == null)
            {
                return new ErrorServiceResponse(0, null, "", "");
            }
            var response = new ServiceResponse();

            var info = await _authenBL.GetInfoFromToken(token?.ToString());

            if (info == null)
            {
                response.OnError(new ErrorResponse() { Data = info });
            }
            else
            {
                response.OnSuccess(info);
            }

            return Ok(response);
        }
    }
}
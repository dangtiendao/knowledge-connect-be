using KnowledgeConnect.BL;
using KnowledgeConnect.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeConnect.API.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserBL _userBL;
        public UsersController(IUserBL userBL) : base(userBL)
        {
            _userBL = userBL;
            this.CurrentModelType = typeof(User);
            this.BaseBL = userBL;
        }

    }
}

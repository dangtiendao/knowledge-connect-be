using Database.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeConnect.BL
{
    public class UserBL : BaseBL, IUserBL
    {
        private IUserBL _userBL;
        public UserBL(IConfiguration configuration, IDatabaseService databaseService) : base(configuration, databaseService)
        {
            _userBL = (IUserBL?)databaseService;
        }
        
    }
}

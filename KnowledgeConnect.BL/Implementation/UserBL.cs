using Database.Services;
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
        public UserBL(IDatabaseService databaseService) : base(databaseService)
        {
            
        }
        
    }
}

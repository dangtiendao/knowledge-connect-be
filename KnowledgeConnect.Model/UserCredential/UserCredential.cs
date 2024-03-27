using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeConnect.Model
{
    public class UserCredential : BaseModel
    {
        public int UserCredentialsID { get; set; }
        public int UserID { get; set; }
        public string? Password { get; set; }
    }
}

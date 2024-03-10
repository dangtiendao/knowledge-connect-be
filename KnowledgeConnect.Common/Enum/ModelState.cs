using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeConnect.Common.Enum
{
    public enum ModelState
    {
        None = 0,
        Insert = 1,
        Update = 2,
        Delete = 3,
        Duplicate = 4,
        Restore = 5,
        Sync = 6,
        Merge = 7
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeConnect.Common.Enum
{
    /// <summary>
    /// Kiểu validate
    /// </summary>
    public enum ValidateType
    {
        /// <summary>
        /// Bắt buộc
        /// </summary>
        Required = 0,

        /// <summary>
        /// validate trùng - Duy nhất
        /// </summary>
        Unique = 1,

        /// <summary>
        /// Số ký tự max
        /// </summary>
        Maxlenght = 2,

        /// <summary>
        /// Số ký tự min
        /// </summary>
        Minlenght = 3,

        /// <summary>
        /// Email
        /// </summary>
        Email = 4,
        Invalid = 5
    }
}

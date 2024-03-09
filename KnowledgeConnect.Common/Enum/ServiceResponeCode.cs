using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeConnect.Common.Enum
{
    public enum ServiceResponeCode
    {
        /// <summary>
        /// Thành công
        /// </summary>
        Success = 0,

        /// <summary>
        /// Không có quyền
        /// </summary>
        NotPermission = 1,

        /// <summary>
        /// Hết hạn license
        /// </summary>
        LicenseExpire = 2,

        /// <summary>
        /// Dữ liệu không tồn tại
        /// </summary>
        NotFound = 3,

        /// <summary>
        /// Lỗi
        /// </summary>
        Error = 99,

        /// <summary>
        /// Lỗi hệ thống
        /// </summary>
        Exception = 999,


    }
}

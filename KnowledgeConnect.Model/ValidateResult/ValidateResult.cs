using KnowledgeConnect.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeConnect.Model
{
    public class ValidateResult
    {
        /// <summary>
        /// ID bản ghi lỗi
        /// </summary>
        public object ID { get; set; }

        /// <summary>
        /// Mã lỗi
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Nội dung lỗi
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Dữ liệu tùy biến mang thêm
        /// </summary>
        public object AdditionInfo { get; set; }

        public ValidateType ValidateType { get; set; }
    }
}

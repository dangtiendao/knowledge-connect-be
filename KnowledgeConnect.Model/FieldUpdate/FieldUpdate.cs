using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeConnect.Model
{
    /// <summary>
    /// Thông tin field update
    /// </summary>
    public class FieldUpdate
    {
        /// <summary>
        /// Tên model
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Trường khóa chính
        /// </summary>
        public string FieldKey { get; set; }

        /// <summary>
        /// Giá trị trường khóa chính
        /// </summary>
        public object ValueKey { get; set; }

        /// <summary>
        /// Danh sách các cột update
        /// Key - Value
        /// </summary>
        public Dictionary<string, object> FieldNameAndValue { get; set; }

        /// <summary>
        /// Dữ liệu truyefn thêm
        /// </summary>
        public object DataModel { get; set; }
    }
}

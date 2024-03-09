using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeConnect.Model
{
    /// <summary>
    /// Thông tin phân trang
    /// </summary>
    public class PagingRequest
    {
        /// <summary>
        /// Số bản ghi/trang
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Vị trí trang
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Các cột cần select
        /// </summary>
        public string Columns { get; set; }

        /// <summary>
        /// String filter
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// String sort
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// Sử dụng stored procedure
        /// </summary>
        public bool UseSp { get; set; }

        /// <summary>
        /// Tham số tìm kiếm nhanh
        /// </summary>
        public QuickSearch QuickSearch { get; set; }

        /// <summary>
        /// Tham số truyền thêm 
        /// </summary>
        public Dictionary<string, object> CustomParam { get; set; }
    }

    /// <summary>
    /// Tìm kiếm nhanh
    /// </summary>
    public class QuickSearch
    {
        /// <summary>
        /// Giá trị tìm kiếm
        /// </summary>
        public string SearachValue { get; set; }

        /// <summary>
        /// Các cột tìm kiếm
        /// </summary>
        public string[] Columns { get; set; }
    }
}

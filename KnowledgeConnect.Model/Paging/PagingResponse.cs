using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeConnect.Model
{
    public class PagingResponse
    {
        /// <summary>
        /// Data trả về
        /// </summary>
        public object? PageData { get; set; }

        /// <summary>
        /// Tổng số
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Data trả thêm
        /// </summary>
        public object CustomData { get; set; }

        /// <summary>
        /// Khởi tạo
        /// </summary>
        public PagingResponse() { }

        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        public PagingResponse(object pageData, int total, object cusomData)
        {
            PageData = pageData;
            Total = total;
            CustomData = cusomData;
        }
    }
}

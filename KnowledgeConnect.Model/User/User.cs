using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeConnect.Model
{
    [Table("user")]
    public class User : BaseModel
    {
        /// <summary>
        /// ID user
        /// </summary>
        [Key]
        public Guid UserID { get; set; }

        /// <summary>
        /// Mã user
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// Tài khoản user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Tên user
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }
    }
}

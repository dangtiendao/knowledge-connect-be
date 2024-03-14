using KnowledgeConnect.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeConnect.Model
{
    [Table("users")]
    public class User : BaseModel
    {
        /// <summary>
        /// ID user
        /// </summary>
        [Key]
        public int UserID { get; set; }

        /// <summary>
        /// Mã user
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Tài khoản user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Tên user
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public EnumGender Gender { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public EnumStatusUser Status { get; set; }

        ///Role - vai trò ...
    }
}

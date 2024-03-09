using KnowledgeConnect.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeConnect.Model
{
    public class BaseModel
    {
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Ngày chỉnh sửa gần nhất
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Trạng thái bản ghi
        /// </summary>
        public ModelState State { get; set; }

        /// <summary>
        /// Dữ liệu trước khi lưu
        /// </summary>
        public string OldData { get; set; }

        /// <summary>
        /// Gán giá trị khóa chính
        /// </summary>
        /// <param name="value"></param>
        public void SetPrimaryKey(string value)
        {
            PropertyInfo[] propertyInfos = this.GetType().GetProperties();

            PropertyInfo propertyInfoKey = null;
            if (propertyInfos != null)
            {
                propertyInfoKey = propertyInfos.SingleOrDefault(info => info.GetCustomAttribute<KeyAttribute>(true) != null);
                if (propertyInfoKey != null)
                {
                    if (propertyInfoKey.PropertyType == typeof(Int32))
                    {
                        propertyInfoKey.SetValue(this, int.Parse(value));
                    }
                    else if (propertyInfoKey.PropertyType == typeof(long))
                    {
                        propertyInfoKey.SetValue(this, long.Parse(value));
                    }
                    else if (propertyInfoKey.PropertyType == typeof(Guid))
                    {
                        propertyInfoKey.SetValue(this, Guid.Parse(value));
                    }
                    else
                    {
                        propertyInfoKey.SetValue(this, value);
                    }

                }
            }
        }

        /// <summary>
        /// Set giá trị khóa chính
        /// </summary>
        public void SetAutoPrimaryKey()
        {
            SetValueAutoPrimaryKey();
        }

        /// <summary>
        /// Set giá trị khóa chính
        /// </summary>
        private void SetValueAutoPrimaryKey()
        {
            PropertyInfo[] propertyInfos = this.GetType().GetProperties();

            PropertyInfo propertyInfoKey = null;
            if (propertyInfos != null)
            {
                propertyInfoKey = propertyInfos.SingleOrDefault(info => info.GetCustomAttribute<KeyAttribute>(true) != null);
                if (propertyInfoKey != null)
                {
                    if (propertyInfoKey.PropertyType == typeof(Int32))
                    {
                        //Số
                    }
                    else if (propertyInfoKey.PropertyType == typeof(long))
                    {
                        //Số
                    }
                    else if (propertyInfoKey.PropertyType == typeof(Guid))
                    {
                        propertyInfoKey.SetValue(this, Guid.NewGuid());
                    }
                    else
                    {
                        //Chuỗi
                    }

                }
            }
        }

        /// <summary>
        /// Lấy giá trị khóa chính
        /// </summary>
        public void GetPrimaryKeyValue()
        {
            GetPrimaryKeyValueDefault();
        }
        private void GetPrimaryKeyValueDefault()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Lấy ra tên khóa chính
        /// </summary>
        /// <returns></returns>
        public string GetKeyProperty()
        {
            var properties = this.GetType().GetProperties();
            var propertyInfo = properties.Where(property => Attribute.IsDefined(property, typeof(KeyAttribute))).FirstOrDefault();
            if(propertyInfo != null)
            {
                return propertyInfo.Name;
            }
            return "";
        }

        /// <summary>
        /// Lấy ra tên bảng
        /// </summary>
        /// <returns></returns>
        public string GetTableName()
        {
            var tableAttribute = (TableAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(TableAttribute));
            return tableAttribute?.Name ?? "";
        }
    }
}

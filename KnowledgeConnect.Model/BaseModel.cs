using KnowledgeConnect.Common.Enum;
using KnowledgeConnect.Common.Utilities;
using Newtonsoft.Json;
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
        /// Danh sách config detail
        /// </summary>
        public List<EntityDetailConfig>? EntityDetailConfigs { get; set; }


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
        public object GetPrimaryKeyValue()
        {
            var properties = this.GetType().GetProperties();
            var propertyInfo = properties.Where(property => Attribute.IsDefined(property, typeof(KeyAttribute))).FirstOrDefault();
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(this);
            }
            return "";
        }
        
        /// <summary>
        /// Lấy kiểu khóa chính
        /// </summary>
        /// <returns></returns>
        public object GetPrimaryKeyType()
        {
            var properties = this.GetType().GetProperties();

            PropertyInfo propertyKeyInfo = null;

            if (properties != null)
            {
                propertyKeyInfo = properties.SingleOrDefault(p => p.GetCustomAttribute<KeyAttribute>(true) != null);

                if (propertyKeyInfo != null)
                {
                    return propertyKeyInfo.PropertyType;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Get giá trị theo thuộc tính
        /// </summary>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        private object? GetValueByAttribute(Type attributeType)
        {
            var properties = this.GetType().GetProperties();

            PropertyInfo propertyKeyInfo = null;

            if (properties != null)
            {
                propertyKeyInfo = properties.SingleOrDefault(p => p.GetCustomAttribute(attributeType, true) != null);

            }
            return propertyKeyInfo.GetValue(this);
        }
        
        /// <summary>
        /// Gán giá trị theo thuộc tính
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="value"></param>
        public void SetValueByAttribute(Type attributeType, object? value)
        {
            var properties = this.GetType().GetProperties();

            PropertyInfo propertyKeyInfo = null;

            if (properties != null)
            {
                propertyKeyInfo = properties.SingleOrDefault(p => p.GetCustomAttribute(attributeType, true) != null);
            }

            if (propertyKeyInfo != null)
            {
                propertyKeyInfo.SetValue(this, Utility.DeserializeObject(Utility.Serialize(value), propertyKeyInfo.PropertyType));
            }

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

    public class EntityDetailConfig
    {
        public string DetailTableName { get; set; }

        public string ForeignKeyName { get; set; }

        /// <summary>
        /// Tên trường danh sách các con
        /// </summary>
        public string PropertyNameOnMaster { get; set; }

        public bool OnDeleteCascade { get; set; }

        public EntityDetailConfig(string detailTableName, string foreignKeyName, string propertyNameOnMaster, bool onDeleteCascade)
        {
            DetailTableName = detailTableName;
            ForeignKeyName = foreignKeyName;
            PropertyNameOnMaster = propertyNameOnMaster;
            OnDeleteCascade = onDeleteCascade;
        }

        public EntityDetailConfig()
        {
        }
    }
}

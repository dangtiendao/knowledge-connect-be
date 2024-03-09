using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KnowledgeConnect.Common.Utilities
{
    public static class Utility
    {
        public static T Deserialize<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                if(typeof(T) == typeof(string))
                {
                    return (T)((object)json);
                }
                throw;
            }
        }

        public static object DeserializeObject(string json, Type type)
        {
            try
            {
                return JsonConvert.DeserializeObject(json,type);
            }
            catch
            {
                if (type == typeof(string))
                {
                    return json;
                }
                throw;
            }
        }

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Lấy giá trị theo key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetValue<T>(this Dictionary<string, object> dictionary, string key)
        {
            if (dictionary.TryGetValue(key, out var value) && value is T result)
            {
                return result;
            }

            // Nếu không tìm thấy hoặc không thể ép kiểu, trả về giá trị mặc định của kiểu T
            return default(T);
        }

        public static object GetValue(this Dictionary<string, object> dictionary, string key)
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                return value;
            }

            // Nếu không tìm thấy, trả về null
            return null;
        }

        #region Chuỗi
        //public static bool ComPa
        #endregion
    }
}

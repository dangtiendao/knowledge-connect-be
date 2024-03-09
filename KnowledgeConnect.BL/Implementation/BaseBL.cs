using KnowledgeConnect.Common.Constant;
using KnowledgeConnect.Common.Utilities;
using KnowledgeConnect.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using KnowledgeConnect.Common;

namespace KnowledgeConnect.BL
{
    public class BaseBL : IBaseBL
    {
        public Task<ServiceResponse> DeleteDataAsync(BaseModel baseModel)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> DeleteDatas(List<string> ids, Type currentModelType)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetAll(Type currentModelType)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetByIDAsync(Type currentModelType, string id)
        {
            var instance = (BaseModel)Activator.CreateInstance(currentModelType);
            var key = instance.GetKeyProperty();

            var pagingRequest = new PagingRequest()
            {
                Columns = "*",
                PageIndex = 2,
                PageSize = 50,
                Filter = $"[\"{key}\",\"=\",\"{id}\"]",
                Sort = "[{\"selector\":\"EmployeeReviewStatus\",\"DESC\": false},{\"selector\":\"PeriodToDate\",\"DESC\": true},{\"selector\":\"PeriodDateType\",\"DESC\": true},{\"selector\":\"PeriodFromDate\",\"DESC\": true}]"
            };
            GetDataPagingAsync(currentModelType, pagingRequest);

            //TODO
            throw new NotImplementedException();
        }

        public Task<object> GetDataPagingAsync(Type currentModelType, PagingRequest pagingRequest)
        {
            var response = new PagingResponse();

            // Select [Column] FROM [Table] WHERE [Where] GenerateSortedString() GeneratePaginationString() 
            var instance = (BaseModel)Activator.CreateInstance(currentModelType);


            var table = GenerateTableNameString(instance);
            var column = GenerateColumnString(pagingRequest.Columns);
            //var whereStr = GenerateWhereString(pagingRequest.Filter);

            var filterTest = "[[\"FullName\",\"contains\",\"Đạo\"],\"AND\",[\"OrganizationUnitID\",\"in\",\"5642;5667\"],\"AND\",[[\"OrganizationUnitIDLevel2\",\"in\",\"7417\"],\"OR\",[\"OrganizationUnitIDLevel2\",\"contains\",\"7417\"]]]";
            var whereStr = GenerateWhereString(filterTest);
            var sortStr = GenerateSortedString(pagingRequest.Sort);
            var pagingStr = GeneratePaginationString(pagingRequest);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Câu where
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        private string GenerateWhereString(string filter)
        {
            var whereStr = "";
            if (filter != null)
            {
                whereStr = "WHERE ";
                var test = ConvertFilterToWhere(filter);
            }
            return whereStr;
        }

        private static string ConvertFilterToWhere(string filterStr)
        {
            JArray filterArray = JsonConvert.DeserializeObject<JArray>(filterStr);

            StringBuilder whereClause = new StringBuilder();
            ParseFilterArray(filterArray, whereClause);

            return whereClause.ToString();
        }

        private static void ParseFilterArray(JArray filterArray, StringBuilder whereClause)
        {
            for (int i = 0; i < filterArray.Count; i++)
            {
                JToken item = filterArray[i];
                if (item is JArray)
                {
                    whereClause.Append("(");
                    ParseFilterArray((JArray)filterArray[i], whereClause);
                    whereClause.Append(")");
                }
                else if (item is JValue)
                {
                    JValue value = (JValue)item;
                    switch (i)
                    {
                        case 0:
                            //Column
                            whereClause.Append(value);
                            break;
                        case 1:
                            //Operator
                            whereClause.Append(GetComparisonOperator(value.ToString()));
                            break;
                        case 2:
                            //Value
                            //Xử lý các giá trị , trường hợp đặc biệt của value
                            var valueStr = value.ToString();
                            if (value.Type == JTokenType.Date)
                            {
                                valueStr = $"{((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss")}";
                            }
                            if (i == 2)
                            {
                                valueStr = $"'{valueStr}'";
                            }
                            whereClause.Append($"{valueStr}'");
                            break;
                        default:
                            break;
                    }
                    

                }
                whereClause.Append(" ");
            }
        }

        static string GetComparisonOperator(string op)
        {
            switch (op.ToUpper())
            {
                case Operator.Equal: return "= {0}";
                case Operator.NotEqual: return "<> {0}";
                case Operator.GreaterThan: return "> {0}";
                case Operator.GreaterOrEqual: return ">= {0}";
                case Operator.LessThan: return "< {0}";
                case Operator.LessOrEqual: return "<= {0}";
                case Operator.Contains: 
                case Operator.StartWith:
                case Operator.EndWith:
                    return "LIKE {0}";
                case Operator.And: return "AND";
                case Operator.Or: return "OR";
                case Operator.Is: return "IS";
                case Operator.IsNull: return "IS NULL";
                case Operator.NotNull: return "NOT NULL";
                case Operator.IsNotNull: return "IS NOT NULL";
                case Operator.In: return "IN ({0})";
                case Operator.NotIn: return "NOT IN ({0})";

                default: return op;
            }
        }

        /// <summary>
        /// Tên bảng
        /// </summary>
        /// <returns></returns>
        private string GenerateTableNameString(BaseModel model)
        {
            return model.GetTableName();
        }

        /// <summary>
        /// Column
        /// </summary>
        /// <returns></returns>
        private string GenerateColumnString(string columns)
        {
            var columnsStr = "*";
            if (columns != null)
            {
                ///Xử lý thêm
                columnsStr = columns;
            }
            return columnsStr;
        }

        /// <summary>
        /// chuỗi sắp xếp
        /// </summary>
        /// <returns></returns>
        private string GenerateSortedString(string sort)
        {
            var sortStr = "";
            if (sort != null)
            {
                var sorts = new List<string>();
                var objectSorts = Utility.Deserialize<List<Dictionary<string, object>>>(sort);
                foreach (var objectSort in objectSorts)
                {
                    var selector = objectSort.GetValue<string>("selector");
                    var desc = objectSort.GetValue<bool>("DESC");
                    var sortingAlgorithm = desc ? SortConstants.DESC : SortConstants.ASC;
                    sorts.Add($" {selector} {sortingAlgorithm} ");
                }
                sortStr = $"ORDER BY {string.Join(",", sorts)} ";
            }
            return sortStr;
        }

        /// <summary>
        /// Chuỗi Phân trang
        /// </summary>
        /// <returns></returns>
        private string GeneratePaginationString(PagingRequest pagingRequest)
        {
            var pagingStr = "";
            if (pagingRequest.PageSize == -1)
            {
                return pagingStr;
            }
            if (pagingRequest.PageSize > 0 && pagingRequest.PageIndex > 0)
            {
                var pageSize = pagingRequest.PageSize > 5000 ? 5000 : pagingRequest.PageSize;
                var pageIndex = pagingRequest.PageIndex;
                pagingStr = $"LIMMIT {pageSize} OFFSET {(pageIndex - 1) * pageSize} ";
            }
            return pagingStr;
        }

        public Task<object> GetDetailByMasterID(Type currentModelType, string id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> SaveDataAsync(BaseModel baseModel)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> SaveListDataAsync(List<BaseModel> baseModels)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> UpdateFieldAsync(FieldUpdate fieldUpdate)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> UpdateFieldsAsync(Type currentModelType, List<FieldUpdate> fieldUpdates)
        {
            throw new NotImplementedException();
        }
    }
}


using System.Text.Json;
using System.Text;
using Dapper;
using UnpakCbt.Common.Application.SortAndFilter;
using System.Linq;
using System.Collections.Generic;

namespace UnpakCbt.Common.Application.Pagingnation
{
    public class DynamicQueryBuilder
    {
        private readonly List<string> _conditions = new();
        private readonly DynamicParameters _parameters = new();
        private bool isGlobalSearch = false;

        public void ApplySearchFilters<TQuery>(TQuery request, List<SearchColumn> allowSearch)
        where TQuery : ISearchable
        {
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                isGlobalSearch = true;
                foreach (SearchColumn field in allowSearch)
                {
                    AddCondition(field.Val.ToString(), "LIKE", request.SearchTerm, true);
                }
            } 
            else if (request.SearchColumn?.Any() == true)
            {
                foreach (var column in request.SearchColumn)
                {
                    SearchColumn? search = allowSearch.FirstOrDefault(sc => sc.Key == column.Key);

                    if (search == null || search.Key == "string")
                        continue; // Skip jika tidak ditemukan di allowSearch atau key-nya "string"

                    if (column.Val is JsonElement je)
                    {
                        switch (column.Type.ToLower())
                        {
                            case "text" when je.ValueKind == JsonValueKind.String:
                                var value = JsonSerializer.Deserialize<string>(je.GetRawText());
                                AddCondition(search.Val.ToString(), "LIKE", value, true);
                                break;

                            case "range" when je.ValueKind == JsonValueKind.Array && column.Key != "string":
                                var list = JsonSerializer.Deserialize<List<string>>(je.GetRawText());
                                AddInCondition(search.Val.ToString(), list);
                                break;

                            case "between" when je.ValueKind == JsonValueKind.Array:
                                var listBetween = JsonSerializer.Deserialize<List<string>>(je.GetRawText());
                                if (listBetween.Count == 2 && column.Key != "string")
                                {
                                    AddBetweenCondition(search.Val.ToString(), listBetween[0], listBetween[1]);
                                }
                                break;
                        }
                    }
                    else if (column.Val is List<string> valuesList)
                    {
                        switch (column.Type.ToLower())
                        {
                            case "range" when valuesList.Count > 0 && column.Key != "string":
                                AddInCondition(search.Val.ToString(), valuesList);
                                break;

                            case "between" when valuesList.Count == 2 && column.Key != "string":
                                AddBetweenCondition(search.Val.ToString(), valuesList[0], valuesList[1]);
                                break;
                        }
                    }
                }
            }
        }


        public void ApplySorting<TQuery>(StringBuilder sql, TQuery request)
        where TQuery : ISearchable
        {
            if (request.SortColumn?.Any() == true)
            {
                var orderBy = string.Join(", ", request.SortColumn
                                .Where(s => s.Key != "string")
                                .Select(s => $"{s.Key} {(s.Val.ToUpper() == "DESC" ? "ASC" : "DESC")}"));

                sql.Append($" ORDER BY {orderBy}");
            }
            else
            {
                sql.Append(" ORDER BY uuid ASC");
            }
        }

        public void AddCondition(string column, string operation, object value, bool isLike = false)
        {
            var paramName = $"@{column.Replace(".", "_")}";

            if (isLike && value is string strValue)
            {
                _parameters.Add(paramName, $"%{strValue}%");
            }
            else
            {
                _parameters.Add(paramName, value);
            }

            _conditions.Add($"{column} {operation} {paramName}");
        }

        private void AddInCondition(string column, List<string>? values)
        {
            if (values == null || values.Count == 0) return;

            var paramNames = new List<string>();
            for (int i = 0; i < values.Count; i++)
            {
                var paramName = $"@{column.Replace(".", "_")}_{i}";
                paramNames.Add(paramName);
                _parameters.Add(paramName, values[i]);
            }

            _conditions.Add($"{column} IN ({string.Join(", ", paramNames)})");
        }

        public void AddBetweenCondition(string column, object value1, object value2)
        {
            var param1 = $"@{column.Replace(".", "_")}_1";
            var param2 = $"@{column.Replace(".", "_")}_2";
            _conditions.Add($"{column} BETWEEN {param1} AND {param2}");
            _parameters.Add(param1, value1);
            _parameters.Add(param2, value2);
        }

        public string BuildWhereClause()
        {
            return _conditions.Any() ? " WHERE " + string.Join(isGlobalSearch? " OR " : " AND ", _conditions) : "";
        }

        public DynamicParameters GetParameters() => _parameters;
    }
}

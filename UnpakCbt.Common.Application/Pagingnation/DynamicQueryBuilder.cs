
using System.Text.Json;
using System.Text;
using Dapper;
using UnpakCbt.Common.Application.SortAndFilter;

namespace UnpakCbt.Common.Application.Pagingnation
{
    public class DynamicQueryBuilder
    {
        private readonly List<string> _conditions = new();
        private readonly DynamicParameters _parameters = new();

        public void ApplySearchFilters<TQuery>(TQuery request, List<string> allowSearch)
        where TQuery : ISearchable
        {
            if (request.SearchColumn?.Any() == true)
            {
                foreach (var column in request.SearchColumn)
                {
                    switch (column.Type.ToLower())
                    {
                        case "text":
                            AddCondition(column.Key, "LIKE", $"%{column.Val}%");
                            break;
                        case "range":
                            if (column.Val is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
                            {
                                var list = JsonSerializer.Deserialize<List<string>>(jsonElement.GetRawText());
                                AddInCondition(column.Key, list);
                            }
                            else if (column.Val is List<string> valuesList && valuesList.Count > 0)
                            {
                                AddInCondition(column.Key, valuesList);
                            }
                            break;
                        case "between":
                            if (column.Val is JsonElement jsonElement1 && jsonElement1.ValueKind == JsonValueKind.Array)
                            {
                                var list = JsonSerializer.Deserialize<List<string>>(jsonElement1.GetRawText());
                                if (list != null && list.Count == 2)
                                {
                                    AddBetweenCondition(column.Key, list[0], list[1]);
                                }
                            }
                            else if (column.Val is List<string> valuesList && valuesList.Count == 2)
                            {
                                AddBetweenCondition(column.Key, valuesList[0], valuesList[1]);
                            }
                            else
                            {
                                throw new ArgumentException($"Invalid value for 'between' filter on column {column.Key}");
                            }
                            break;
                    }
                }
            }
            else if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                foreach (var field in allowSearch)
                {
                    AddCondition(field, "LIKE", $"%{request.SearchTerm}%");
                }
            }
        }


        public void ApplySorting<TQuery>(StringBuilder sql, TQuery request)
        where TQuery : ISearchable
        {
            if (request.SortColumn?.Any() == true)
            {
                var orderBy = string.Join(", ", request.SortColumn.Select(s => $"{s.Key} {s.Val.ToUpper()}"));
                sql.Append($" ORDER BY {orderBy}");
            }
            else
            {
                sql.Append(" ORDER BY uuid ASC");
            }
        }

        private void AddCondition(string column, string operation, object value)
        {
            var paramName = $"@{column.Replace(".", "_")}";
            _conditions.Add($"{column} {operation} {paramName}");
            _parameters.Add(paramName, value);
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

        private void AddBetweenCondition(string column, object value1, object value2)
        {
            var param1 = $"@{column.Replace(".", "_")}_1";
            var param2 = $"@{column.Replace(".", "_")}_2";
            _conditions.Add($"{column} BETWEEN {param1} AND {param2}");
            _parameters.Add(param1, value1);
            _parameters.Add(param2, value2);
        }

        public string BuildWhereClause()
        {
            return _conditions.Any() ? " WHERE " + string.Join(" AND ", _conditions) : "";
        }

        public DynamicParameters GetParameters() => _parameters;
    }
}

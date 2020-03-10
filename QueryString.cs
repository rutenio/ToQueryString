using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Collections;
using System;

namespace Vigilate.Helpers
{
    public static class QueryString
    {
        public static string ToQueryString(object obj)
        {
            var result = new List<string>();
            ReadObj(obj, ref result);
            return string.Join("&", result.ToArray());
        }

        private static void ReadObj(object obj, ref List<string> result, string prefix = null)
        {
            var props = obj.GetType().GetProperties().Where(p => p.GetValue(obj, null) != null);
            foreach (var p in props)
            {
                var value = p.GetValue(obj, null);
                var type = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                if (type.IsArray)
                {
                    var cnt = 0;
                    foreach (var item in value as IEnumerable)
                    {
                        ReadObj(item, ref result, $"{prefix += p.Name}[{cnt}]");
                        cnt++;
                    }
                }
                else if (type == typeof(object))
                {
                    ReadObj(value, ref result, prefix += p.Name);
                }
                else
                {
                    result.Add($"{prefix}{(string.IsNullOrEmpty(prefix) ? p.Name : $"[{p.Name}]")}={value.ToString()}");
                }
            }
            return;
        }
    }
}

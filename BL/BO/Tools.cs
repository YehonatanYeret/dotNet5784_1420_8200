namespace BO;
using System.Reflection;

static class Tools
{
    public static string ToStringProperty<T>(this T obj, int indent = 0)
    {
        if (obj == null)
            return "Not Set";

        Type type = obj.GetType();
        if (type.IsPrimitive || type.IsValueType || type == typeof(string))
            return obj.ToString()!;

        string result = "";
        if (obj is System.Collections.IEnumerable enumerable)
        {
            result += $"\n{new string(' ', indent)}[\n";
            foreach (var item in enumerable)
                result += item.ToStringProperty(indent + 1);
            result += $"{new string(' ', indent)}]";
        }
        else
        {

            result += $"{new string(' ', indent)}{{\n";
            foreach (PropertyInfo property in obj.GetType().GetProperties())
                result += new string(' ', indent + 1) + property.Name + ": " + property.GetValue(obj).ToStringProperty(indent + 1) + '\n';
            result += new string(' ', indent) + "}\n";
        }
        return result;

    }
}

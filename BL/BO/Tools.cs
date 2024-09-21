namespace BO
{
    using System.Reflection;

    /// <summary>
    /// A static class containing utility methods.
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Converts the properties of an object to a formatted string.
        /// </summary>
        /// <typeparam name = "T" > The type of the object.</typeparam>
        /// <param name = "obj" > The object to convert.</param>
        /// <param name = "indent" > The number of spaces for indentation(default is 0).</param>
        /// <returns>A formatted string representation of the object's properties.</returns>
        public static string ToStringProperty<T>(this T obj, int indent = 0)
        {
            if (obj == null)// if the object is null
                return "Not Set";

            Type type = obj.GetType();
            if (type.IsPrimitive || type.IsValueType || type == typeof(string))// if the object is a primitive type
                return obj.ToString()!;

            string result = "";
            if (obj is System.Collections.IEnumerable enumerable)// if the object is a collection
            {
                result += $"\n{new string(' ', indent)}[\n";
                foreach (var item in enumerable)// for each item in the collection convert it to string
                    result += item.ToStringProperty(indent + 1);
                result += $"{new string(' ', indent)}]";
            }
            else// if the object is not a collection
            {
                result += $"{new string(' ', indent)}\n";
                foreach (PropertyInfo property in obj.GetType().GetProperties())
                    result += new string(' ', indent + 1) + property.Name + ": " + property.GetValue(obj).ToStringProperty(indent + 1) + '\n';
                result += new string(' ', indent) + "\n";
            }
            return result;
        }
    }
}

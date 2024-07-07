using System.Reflection;
using System.Text.RegularExpressions;

namespace MacSave.Funcs
{
    public class SanetizerInputs
    {
        private static readonly Regex SanitizeRegex = new("[^a-zA-Z0-9 @.,;:!?-]");

        public static string? InputSanitizer(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            return SanitizeRegex.Replace(input, string.Empty);
        }

        public void IterateProperties<T>(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            Type type = obj.GetType();

            foreach (
                PropertyInfo property in type.GetProperties(
                    BindingFlags.Public | BindingFlags.Instance
                )
            )
            {
                string propertyName = property.Name;
                object propertyValue = property.GetValue(obj);

                Console.WriteLine("------------------------");
                Console.WriteLine("\nProperties Without Clean: \n");
                Console.WriteLine($"Property: {propertyName}, \nValue: {propertyValue} \n");

                Console.WriteLine("------------------------");
                Console.WriteLine("Start clean items\n");

                string propertyNameClean = InputSanitizer(propertyName);
                string propertyValueClean = InputSanitizer(
                    propertyValue?.ToString() ?? string.Empty
                );

                Console.WriteLine(
                    $"Clean Property: {propertyNameClean}, \nClean Value: {propertyValueClean}"
                );
            }
        }
    }
}

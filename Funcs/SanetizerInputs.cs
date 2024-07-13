using System.Reflection;
using System.Text.RegularExpressions;

namespace MacSave.Funcs.RegexSanitizer
{
    public class SanetizerInputs
    {
        private static readonly Regex SanitizeRegex = new("[^a-zA-Z0-9 @.,;:!?-]");

        //as private
        private static string? InputSanitizer(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            return SanitizeRegex.Replace(input, string.Empty).Trim();
        }

        public async Task<T> IterateProperties<T>(T obj, bool ActiveLogginObject)
            where T : new()
        {
            T _newObjectGenericClean = new();
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            Type type = obj.GetType();

            // iterate whole object and its public properties
            foreach (
                PropertyInfo property in type.GetProperties(
                    BindingFlags.Public | BindingFlags.Instance
                )
            )
            {
            await Task.Delay(10);

                // get name and value of propertie
                string propertyName = property.Name;
                object? propertyValue = property.GetValue(obj);

                // call sanitizer func regex and return clean value
                string? propertyNameClean = InputSanitizer(propertyName);
                string? propertyValueClean = InputSanitizer(
                    propertyValue?.ToString() ?? string.Empty
                );
                
                //must to check is value if diferent than string
                
        
                //take value to be set at place of dirty field
                property.SetValue(
                    _newObjectGenericClean,
                    Convert.ChangeType(propertyValueClean, property.PropertyType)
                );

                // if true, loggin is active
                if (ActiveLogginObject)
                {
                    Console.WriteLine("------------------------");
                    Console.WriteLine("\nProperties Without Clean: \n");
                    Console.WriteLine($"Property: {propertyName}, \nValue: {propertyValue} \n");
                    Console.WriteLine("------------------------");
                    Console.WriteLine("Start clean items\n");
                    Console.WriteLine(
                        $"Clean Property: {propertyNameClean}, \nClean Value: {propertyValueClean}"
                    );
                }
            }

            return _newObjectGenericClean;
        }
    }
}

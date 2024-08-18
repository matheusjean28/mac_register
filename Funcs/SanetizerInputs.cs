using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

        // public async Task<T> IterateProperties<T>(T obj, bool activeLogging)
        // {
        //     // Get type of obj
        //     Type type = obj.GetType();

        //     // Get all properties of the object
        //     PropertyInfo[] properties = type.GetProperties();

        //     foreach (PropertyInfo property in properties)
        //     {
        //         // Get the name and value of each property
        //         string propertyName = property.Name;
        //         object propertyValue = property.GetValue(obj);
        //         propertyValue.ToString();
        //         if(propertyValue != null){

        //             Console.WriteLine($"\n\n\n{propertyName}: \n{propertyValue} \n\n\n" );
        //            property.SetValue(obj, InputSanitizer(), null);
        //             Console.WriteLine($"\n\n\n{propertyName}: \n{propertyValue} \n\n\n" );

        //         }

        //     }

        //     // Perform any additional processing if required

        //     // Return the modified object (if any modifications are done)
        //     return await Task.FromResult(obj);
        // }
    }
}

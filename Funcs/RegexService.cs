using System.Text.RegularExpressions;
using System.Web;

namespace MacSave.Funcs
{
    public class RegexService
    {
        private static readonly Regex SanitizeRegex = new Regex("[^a-zA-Z0-9 @.,;:!?-]");

        public  string SanitizeInput(string userInput)
        {
            var input = userInput;
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            
            input = SanitizeRegex.Replace(input, string.Empty).Trim();

            input = HttpUtility.HtmlEncode(input);
             input = input.Replace("'", "''");
            return input;
        }
    }
}

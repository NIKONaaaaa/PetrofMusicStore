namespace Petrof.Utility
{
    using System.Text.RegularExpressions;
    public class SimpleInputValidation
    {
        private static readonly string pattern = @"/[a-zA-Z0-9@.\s]*";
        public static bool ValidateString(string input)
        {
            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {
                return true;
            }
            return false;
        }
    }
}
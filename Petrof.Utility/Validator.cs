namespace Petrof.Utility
{
    using System.Text.RegularExpressions;
    public class Validator
    {
        public readonly static string Regular = @"[!@#$%^&*()\-_=+;:\'<,>./?\\\|]+";
        public readonly static string Email = @"^[A-Za-z]+[_.-]*?[A-Za-z]+[_.-]*?[A-Za-z]+@[A-Za-z]+-?[A-Za-z]+\.[A-Za-z]+-?[A-Za-z]+(\.[A-Za-z]+-?[A-Za-z]+)?";
        public readonly static string Number = @"^[\d]+";
        public static bool Parse(string input, string pattern)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            Match match = Regex.Match(input, pattern);
            return match.Success;
        }
    }
}
//Name - Letters and spaces
//Phone Number - Digits
//Email Address - asd@asd.com
//Street - Letters, spaces and digits
//City - Letters and spaces
//State - Letters and spaces
//Postal Code - Digits
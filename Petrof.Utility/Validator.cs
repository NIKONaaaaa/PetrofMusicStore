namespace Petrof.Utility
{
    using System.Text.RegularExpressions;
    public class Validator
    {
        public readonly static string Regular = @"[!@#$%^&*()\-_=+;:\'<,>./?\\\|]+";
        public readonly static string Email = @"^[A-Za-z]+[_.-]*?[A-Za-z]+[_.-]*?[A-Za-z]+@[A-Za-z]+-?[A-Za-z]+\.[A-Za-z]+-?[A-Za-z]+(\.[A-Za-z]+-?[A-Za-z]+)?";
        public readonly static string Number = @"[\D]+";
        private const string err0 = "Success";
        private const string err1 = "This is a required field.";
        private const string err2 = "Field can not contain special characters.";
        private const string err3 = "Email address is invalid.";
        private const string err4 = "Field must only contain numbers";
        public static Dictionary<string, string> Parse(Dictionary<string, string?> attributes)
        {
            Dictionary<string, string> errorMap = new();
            foreach ((string attrib, string? value) in attributes)
            {
                errorMap.Add(attrib, err0);
                if (string.IsNullOrEmpty(value))
                {
                    errorMap[attrib] = err1;
                }
                else
                {
                    errorMap[attrib] = Validate(attrib, value);
                }
            }
            return errorMap;
        }
        private static string Validate(string attrib, string value)
        {
            switch (attrib)
            {
                case "OrderHeader.EmailAddress":
                    Match result = Regex.Match(value, Email);
                    if (!result.Success)
                    {
                        return err3;
                    }
                    return err0;
                case "OrderHeader.PhoneNumber":
                case "OrderHeader.PostalCode":
                    result = Regex.Match(value, Number);
                    if (result.Success)
                    {
                        return err4;
                    }
                    return err0;
                default:
                    result = Regex.Match(value, Regular);
                    if (result.Success)
                    {
                        return err2;
                    }
                    return err0;
            }
        }
    }
}
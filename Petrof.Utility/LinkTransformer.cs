namespace Petrof.Utility
{
    public class LinkTransformer
    {
        public static string Transform(string? input)
        {
            if (input == null)
            {
                return "";
            }
            if (input.Contains("youtube"))
            {
                return input.Replace("watch?v=", "embed/");
            }
            return "";
        }
    }
}
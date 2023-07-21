namespace Petrof.Utility
{
    using Newtonsoft.Json.Linq;
    using System.Globalization;
    using System.Net;
    public class LocalizationSetter
    {
        public string DefaultCurrency { get; set; }
        public CultureInfo DefaultCulture { get; set; }

        public const string Euro = "eur";
        public const string AmericanDollar = "usd";
        public const string CanadianDollar = "cad";
        public const string AustralianDollar = "aud";
        public const string BritishPound = "gbp";

        public const string EULocal = "fr-FR";
        public const string USALocal = "en-US";
        public const string CANLocal = "en-CA";
        public const string AUSLocal = "en-AU";
        public const string UKLocal = "en-GB";

        //public static string GetCountryByIP()
        //{
        //    string info = new WebClient().DownloadString("http://ipinfo.io");
        //    var jsonObject = JObject.Parse(info);
        //    var country = jsonObject.Value<string>("country");

        //    return country;
        //}
    }
}
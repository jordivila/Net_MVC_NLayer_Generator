using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace $customNamespace$.Models.Globalization
{
    public static class GlobalizationHelper
    {
        public static string SpanishInternacional = "es";
        public static string EnglishUS = "en-US";
        public static string EnglishGB = "en-GB";

        public static List<CultureInfo> CultureInfoAvailableList()
        {
            List<CultureInfo> result = new List<CultureInfo>() { 
                CultureInfo.GetCultureInfo(GlobalizationHelper.SpanishInternacional),
                CultureInfo.GetCultureInfo(GlobalizationHelper.EnglishUS),
                CultureInfo.GetCultureInfo(GlobalizationHelper.EnglishGB),
            };

            return result;
        }

        public static CultureInfo CultureInfoGetOrDefault(string culture)
        {
            CultureInfo defaultCulture = CultureInfo.GetCultureInfo(GlobalizationHelper.SpanishInternacional);

            if (string.IsNullOrEmpty(culture))
            {
                return defaultCulture;
            }
            else
            {

                var x = from c in GlobalizationHelper.CultureInfoAvailableList()
                        where c.Name.ToLower() == culture.ToLower()
                        select c;

                if (x.Count() > 0)
                {
                    return x.First();
                }
                else
                {
                    return defaultCulture;
                }
            }
        }
    }
}

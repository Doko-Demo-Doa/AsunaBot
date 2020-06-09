using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace NadekoBot.Core.Common
{
    public class URLUtils
    {
        public URLUtils()
        {
        }

        public static string ExtractUrl(string InputStr)
        {
            MatchCollection matches = Regex.Matches(InputStr, @"\b((https?|ftp|file)://|(www|ftp)\.)[-A-Z0-9+&@#/%?=~_|$!:,.;]*[A-Z0-9+&@#/%=~_|$]", RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                if (match.Value != null)
                {
                    return match.Value;
                }
            }
            return "";
        }

        public static bool IsImageUrl(string URL)
        {
            try
            {
                var req = (HttpWebRequest)HttpWebRequest.Create(URL);
                req.Method = "HEAD";
                using var resp = req.GetResponse();
                return resp.ContentType.ToLower(CultureInfo.InvariantCulture)
                           .StartsWith("image/");
            }
            catch (WebException e)
            {
                // Code...
                return false;
            }
        }
    }
}

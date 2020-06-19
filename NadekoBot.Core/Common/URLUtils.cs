using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace NadekoBot.Core.Common
{
    public class URLUtils
    {
        public URLUtils()
        {
        }

        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG", "MP4", "OGG", "MKV", "WEBM" };

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
                foreach (var ext in ImageExtensions)
                {
                    if (URL.ToUpperInvariant().EndsWith(ext))
                    {
                        return true;
                    }
                }

                var req = (HttpWebRequest)HttpWebRequest.Create(URL);
                req.Method = "HEAD";
                using var resp = req.GetResponse();
                return resp.ContentType.ToLower(CultureInfo.InvariantCulture)
                           .StartsWith("image/");
            }
            catch (WebException)
            {
                // Code...
                return false;
            }
        }

        public static Stream GetStreamFromUrl(string url)
        {
            byte[] imageData = null;

            using (var wc = new WebClient())
                imageData = wc.DownloadData(url);

            return new MemoryStream(imageData);
        }
    }
}

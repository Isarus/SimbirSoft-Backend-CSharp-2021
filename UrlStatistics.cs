using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace Backend_1
{
    public class UrlStatistics
    {
        protected const string codeFileName = "file.html";
        public IUrlStatisticsMethod statisticsMethod;

        public string GetStatistics(Uri uri)
        {
            string htmlCode = GetHTMLCode(uri.AbsoluteUri);
            return statisticsMethod.GetStatistics(htmlCode);
        }

        public string GetStatistics(string htmlCode)
        {
            return statisticsMethod.GetStatistics(htmlCode);
        }

        public bool TryGetStatistics(Uri uri, out string statistics)
        {
            try
            {
                string htmlCode = GetHTMLCode(uri.AbsoluteUri);
                statistics = statisticsMethod.GetStatistics(htmlCode);
                return true;
            }
            catch (WebException)
            {
                statistics = "";
                return false;
            }
            catch (NullReferenceException)
            {
                statistics = "";
                return false;
            }
        }

        public bool VerifyURL(string url)
        {
            string pattern = @"^https?:\/\/(?:www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b[-a-zA-Z0-9()@:%_\+.~#?&//=]*$";
            Regex rgx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return rgx.IsMatch(url);
        }

        private string GetHTMLCode(string url)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.DownloadFile(url, codeFileName);
            return File.ReadAllText(codeFileName);
        }
    }
}
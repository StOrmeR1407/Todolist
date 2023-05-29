using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Documents;

namespace botKMT
{
    internal class ThoiTiet
    {
        public static WebClient webClient = new WebClient();
        public static string url = "https://www.nchmf.gov.vn/kttv/";
        public static string html = webClient.DownloadString(url);
        public static Dictionary<string, string> linkDataDictionary = ExtractLinkData(html);

        //public static string pattern = "<a\\s+class=\"name-wt-city\"\\s+href=\"(.*?)\"[^>]*>(.*?)</a>";
        //public static MatchCollection matches = Regex.Matches(html, pattern);

        public static Dictionary<string, string> ExtractLinkData(string websiteContent)
        {
            Dictionary<string, string> linkDataDictionary = new Dictionary<string, string>();

            string pattern = @"<a\s+class=""name-wt-city""\s+href=""([^""]*?)""[^>]*>(.*?)</a>";
            MatchCollection matches = Regex.Matches(websiteContent, pattern);

            foreach (Match match in matches)
            {
                string href = match.Groups[1].Value;
                string content = match.Groups[2].Value;

                linkDataDictionary.Add(content, href);
            }

            return linkDataDictionary;
        }
        public static string CheckLink(string a)
        {
            foreach (KeyValuePair<string, string> linkData in linkDataDictionary)
            {
                string content = linkData.Key;
                string href = linkData.Value;
                if(a == content)
                {
                    return href;
                    break;
                }    
            }
            return $"không tồn tại tỉnh {a} này";
        }
        public static string GetThoiTiet(string a, string b)
        {
            string link = "";
            string total_content = "";

            foreach (KeyValuePair<string, string> linkData in linkDataDictionary)
            {
                string content = linkData.Key;
                string href = linkData.Value;

                if (a == content)
                {
                    link = href;
                    break;
                }
            }

            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                string html = client.DownloadString(link);

                string pattern = "<div\\s+class=\"uk-width-1-4 uk-first-column\"[^>]*>(.*?)</div>";
                MatchCollection matches = Regex.Matches(html, pattern, RegexOptions.Singleline);
                foreach (Match match in matches)
                {
                    string content_of_web = match.Groups[1].Value;
                    total_content += content_of_web + "\n";
                }
            }

            return total_content;
            }

        public static string Test()
        {
        WebClient webClient = new WebClient();
        string url = "https://nchmf.gov.vn/kttv/";
        string html = webClient.DownloadString(url);
            return html;
        }
        }
    }
           

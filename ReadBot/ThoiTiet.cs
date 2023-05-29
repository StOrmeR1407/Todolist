using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Web;

namespace ReadBot
{
    internal class ThoiTiet
    {
        public static WebClient webClient = new WebClient();
        public static string url = "https://www.nchmf.gov.vn/kttv/";
        public static string html = webClient.DownloadString(url);
        public static Dictionary<string, string> linkDataDictionary = ExtractLinkData(html);
        public static string[] SampleTT = {"Nhiệt độ ", "Thời tiết ", "Độ ẩm ", "Hướng giớ "};

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
                byte[] bytes = Encoding.Default.GetBytes(content);
                content = Encoding.UTF8.GetString(bytes);

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

            WebClient webClient = new WebClient();
            string url = link;
            string html = webClient.DownloadString(url);
            string pattern = @"<div\s+class=""uk-width-3-4"">(.*?)<\/div>";
            MatchCollection matches = Regex.Matches(html, pattern);
            for (int i = 0; i < 4; i++)
            {

                string content = matches[i].Groups[1].Value;
                byte[] bytes = Encoding.Default.GetBytes(content);
                content = Encoding.UTF8.GetString(bytes);
                total_content += SampleTT[i] + content + "\n";
            }
            return total_content;

        }
    
    

        public static string Test()
        {
            string tt = "Thời tiết hôm nay";
        WebClient webClient = new WebClient();
        string url = "https://nchmf.gov.vn/Kttv/vi-VN/1/lai-chau-w64.html";
        string html = webClient.DownloadString(url);
        string pattern = @"<div\s+class=""uk-width-3-4"">(.*?)<\/div>";
        MatchCollection matches = Regex.Matches(html, pattern);
        for (int i = 0; i < 4; i++)
            {
                
            string content = matches[0].Groups[1].Value;
            byte[] bytes = Encoding.Default.GetBytes(content);
            content = Encoding.UTF8.GetString(bytes);
            tt += SampleTT[i] + content + "\n";
                }
            return tt;

        }
    
    }  
}
   
           

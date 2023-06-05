using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Web;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Windows.Forms.LinkLabel;
using HtmlAgilityPack;

namespace ReadBot
{
    internal class GetName
    {
        public static WebClient webClient = new WebClient();
        public static string url = "https://www.facebook.com/antonio.codeca";
        public static string html = webClient.DownloadString(url);

        public static string HTML()
        {
            return html;
        }
        public static string Get() 
        {
            string tt = "Kết quả là: ";
            string pattern = @"<h1[^>]*>(.*?)<\/h1>";
            MatchCollection matches = Regex.Matches(html, pattern);
            if(matches.Count == 0)
            {
                return tt + "Không có kết quả.";
            }
            else
            {
                string content_of_web = matches[0].Groups[1].Value;
                byte[] bytes = Encoding.UTF8.GetBytes(content_of_web);
                content_of_web = Encoding.UTF8.GetString(bytes);
                tt += content_of_web;
                return tt;
            }
            
        }

        public static string Get2() 
        {
            string tt = "Kết quả là";
            
            // Load the HTML string into an HtmlDocument
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(html);

            // Find the <h1> tag with the specified class
            HtmlNode h1Node = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='x1heor9g x1qlqyl8 x1pd3egz x1a2a7pz']");

            // Extract the inner text of the <h1> tag
            string content = h1Node.InnerText;
            tt += content;
            return tt;
        }

    }
}

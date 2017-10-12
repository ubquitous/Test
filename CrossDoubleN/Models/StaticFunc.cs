using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using AngleSharp.Parser.Html;
using AngleSharp.Xml;

namespace CrossDoubleN.Models
{
    public static class StaticFunc
    {
        public static List<string> GetText()
        {

            string html = GetInfo("http://www.nonograms.ru/instructions");
            List<string> b=new List<string>();

            Regex reHref = new Regex(@"(?inx)
    <a \s [^>]*
        href \s* = \s*
            (?<q> ['""] )
                (?<url> [^""]+ )
            \k<q>
    [^>]* >");
            foreach (Match match in reHref.Matches(html))
                b.Add(match.Groups["url"].ToString());
            return b;
        }

        private static string GetInfo(string address)
        {
            HttpWebRequest proxy_request = (HttpWebRequest)WebRequest.Create(address);
            proxy_request.Method = "GET";
            proxy_request.ContentType = "application/x-www-form-urlencoded";
            proxy_request.UserAgent = "Chrome/4.0.249.89";
            proxy_request.KeepAlive = true;
            HttpWebResponse resp = proxy_request.GetResponse() as HttpWebResponse;
            string html = "";
            using (StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
                html = sr.ReadToEnd();
            html = html.Trim();
            return html;
        }

        public static List<string> AS()
        {
            string html = GetInfo("http://www.nonograms.ru/instructions");
            List<string> b = new List<string>();

            var parser = new HtmlParser();
            var angle = parser.Parse(html);
            foreach (var element in angle.QuerySelectorAll("link, a[title]"))
            {
                b.Add(element.GetAttribute("href"));
                if (element.GetAttribute("title") != null)
                {
                    b.Add(element.GetAttribute("title"));
                }
                else
                {
                    b.Add(element.GetAttribute("rel"));
                }
                

            }

        return b;
        }
    }
}
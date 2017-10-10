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

namespace CrossDoubleN.Models
{
    public static class StaticFunc
    {
        public static List<string> GetText()
        {
            HttpWebRequest proxy_request = (HttpWebRequest)WebRequest.Create("http://www.nonograms.ru/instructions");
            proxy_request.Method = "GET";
            proxy_request.ContentType = "application/x-www-form-urlencoded";
            proxy_request.UserAgent = "Chrome/4.0.249.89";
            proxy_request.KeepAlive = true;
            HttpWebResponse resp = proxy_request.GetResponse() as HttpWebResponse;
            string html = "";
            using (StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
                html = sr.ReadToEnd();
            html = html.Trim();
            
            Debug.WriteLine("END");
            Regex reg_for_proxy = new Regex(@"(<h1>(.*)</h1>)|(<ol(.+?)</ol>)|(<p>(.+?)</p>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            //(^p>$z)|((<ol)([^o]*)(ol>))|((<h1>)([^h]*)(h1>))
            MatchCollection collect_math = reg_for_proxy.Matches(html);
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
    }
}
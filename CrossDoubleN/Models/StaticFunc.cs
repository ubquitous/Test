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
        public static void GetText()
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
            string b="";
            foreach (Match a in collect_math)
            {

                Debug.WriteLine(a);
                b += Convert.ToString(a);

            }

            Debug.WriteLine("Next");
            var m_strFilePath = "http://www.nonograms.ru/instructions";
            string xmlStr;
            using (var wc = new WebClient())
            {
                xmlStr = wc.DownloadString(m_strFilePath);
            }
            var xmlDoc = new XmlDocument();
            xmlDoc.Load("http://www.nonograms.ru/instructions");
            XmlNodeList lst=xmlDoc.GetElementsByTagName("p");
            for (int i = 0; i < lst.Count; i++)
            {
                Debug.WriteLine(lst[i].InnerXml);
            }
        }
    }
}
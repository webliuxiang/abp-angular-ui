using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace AutoLocalizationTool.Tools
{
    /// <summary>
    /// 有道云翻译类
    /// </summary>
    public static class YoudaoyunTranslate
    {
        /// <summary> 您的应用ID </summary>
        public const string AppKey = "0d3960f817b3f708";

        /// <summary> 您的应用密钥 </summary>
        public const string AppSecret = "mxIzuoOKVI7YE8KnoHDnzz8ZZuwLAP0P";

        /// <summary> 源语言 </summary>
        public const string FromLanguage = "zh-CHS";

        /// <summary> 目标语言 </summary>
        public const string ToLanguage = "en";


        public static string GetTranslate(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return "";
            }
            var dic = new Dictionary<string, string>();
            var url = "https://openapi.youdao.com/api";
            var salt = DateTime.Now.Millisecond.ToString();
            dic.Add("from", FromLanguage);
            dic.Add("to", ToLanguage);
            dic.Add("signType", "v3");
            var ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            var millis = (long)ts.TotalMilliseconds;
            var curtime = Convert.ToString(millis / 1000);
            dic.Add("curtime", curtime);
            var signStr = AppKey + Truncate(q) + salt + curtime + AppSecret; ;
            var sign = ComputeHash(signStr, new SHA256CryptoServiceProvider());
            dic.Add("q", System.Web.HttpUtility.UrlEncode(q));
            dic.Add("appKey", AppKey);
            dic.Add("salt", salt);
            dic.Add("sign", sign);
            return Post(url, dic);
        }


        private static string ComputeHash(string input, HashAlgorithm algorithm)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashedBytes = algorithm.ComputeHash(inputBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }

        private static string Post(string url, Dictionary<string, string> dic)
        {
            var result = "";
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            var builder = new StringBuilder();
            var i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            var data = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (var reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            var resp = (HttpWebResponse)req.GetResponse();
            if (resp.ContentType.ToLower().Equals("audio/mp3"))
            {
                SaveBinaryFile(resp, "合成的音频存储路径");
            }
            else
            {
                var stream = resp.GetResponseStream();
                if (stream != null)
                {
                    using var reader = new StreamReader(stream, Encoding.UTF8);
                    result = reader.ReadToEnd();
                }
                Console.WriteLine(result);
            }

            if (string.IsNullOrEmpty(result))
            {
                return result;
            }

            var ret = JsonConvert.DeserializeObject<TransResultYouDaoYun>(result);


            return ret.translation[0];

        }

        private static string Truncate(string q)
        {
            if (q == null)
            {
                return null;
            }
            var len = q.Length;
            return len <= 20 ? q : (q.Substring(0, 10) + len + q.Substring(len - 10, 10));
        }

        private static void SaveBinaryFile(WebResponse response, string fileName)
        {
            var filePath = fileName + DateTime.Now.Millisecond.ToString() + ".mp3";
            var buffer = new byte[1024];

            Stream outStream = File.Create(filePath);
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                var inStream = response.GetResponseStream();
                if (inStream != null)
                {
                    int l;
                    do
                    {
                        l = inStream.Read(buffer, 0, buffer.Length);
                        if (l > 0)
                            outStream.Write(buffer, 0, l);
                    }
                    while (l > 0);

                    inStream.Close();
                }
                outStream.Close();
             
            }
            catch
            {
                // ignored
            }
        }
    }



    //如果好用，请收藏地址，帮忙分享。
    public class Dict
    {
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
    }

    public class Webdict
    {
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
    }

    public class TransResultYouDaoYun
    {
        /// <summary>
        /// 
        /// </summary>
        public string tSpeakUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string requestId { get; set; }
        /// <summary>
        /// 慕课模块
        /// </summary>
        public string query { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> translation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string errorCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dict dict { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Webdict webdict { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string l { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isWord { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string speakUrl { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace AutoLocalizationTool.Tools
{
    /// <summary>
    /// 百度翻译sdk
    /// </summary>
    public static class BaiduTranslate
    {

        /// <summary> 改成您的APP ID </summary>
        public const string AppId = "20200221000386710";

        /// <summary> 改成您的密钥 </summary>
        public const string SecretKey = "6DXjXjhzXX6PdF07jwrP";

        /// <summary> 源语言 </summary>
        public const string FromLanguage = "zh";

        /// <summary> 目标语言 </summary>
        public const string ToLanguage = "en";

        /// <summary>
        /// 获取翻译结果
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static string GetTranslate(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return "";
            }
            var rd = new Random();
            var salt = rd.Next(100000).ToString();
            var sign = EncryptString(AppId + q + salt + SecretKey);
            var url = "http://api.fanyi.baidu.com/api/trans/vip/translate?";
            url += "q=" + HttpUtility.UrlEncode(q);
            url += "&from=" + FromLanguage;
            url += "&to=" + ToLanguage;
            url += "&appid=" + AppId;
            url += "&salt=" + salt;
            url += "&sign=" + sign;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 6000;
            var response = (HttpWebResponse)request.GetResponse();
            var myResponseStream = response.GetResponseStream();
            var myStreamReader = new StreamReader(myResponseStream!, Encoding.GetEncoding("utf-8"));
            var retString = myStreamReader.ReadToEnd();

            var ret = JsonConvert.DeserializeObject<BaiDuResult>(retString).Trans_result[0];

            myStreamReader.Close();
            myResponseStream.Close();

            return ret.Dst;
        }


        /// <summary>
        /// 计算MD5值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncryptString(string str)
        {
            var md5 = MD5.Create();
            // 将字符串转换成字节数组
            var byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            var byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            var sb = new StringBuilder();
            foreach (var b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }

    }




    public class TransResultItem
    {
        /// <summary>
        /// 你好
        /// </summary>
        public string Src { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Dst { get; set; }
    }

    public class BaiDuResult
    {
        /// <summary>
        /// 
        /// </summary>
        public List<TransResultItem> Trans_result { get; set; }
    }
}

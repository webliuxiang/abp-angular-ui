using System.Collections.Generic;
using System.IO;
using System.Text;
using AutoLocalizationTool.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AutoLocalizationTool
{
    class Program
    {
        /// <summary> 翻译文件路径 </summary>
        public const string FromUrl = "../../../../../\\src\\LTMCompanyName.YoyoCmsTemplate.Core\\Localization\\SourceFiles\\json\\YoyoCmsTemplate-zh-Hans.json";


        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            //读取要翻译的Json文件

            var jsonStr = GetFileJson();
            var jsonData = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonStr);


            var retDic = new Dictionary<string,string>();
            foreach (var temp in jsonData)
            {
                //百度翻译 (注意，进去配置秘钥等信息)
                var from = BaiduTranslate.GetTranslate(temp.Value);

                //有道云翻译(注意，进去配置秘钥等信息)
                //var from = YoudaoyunTranslate.GetTranslate(temp.Value);


                retDic.TryAdd(temp.Key, from);
            }

            var retJson = JsonConvert.SerializeObject(retDic);

            
            File.WriteAllText(@"..\..\..\baidu.json", retJson, Encoding.UTF8);
        }

        /// <summary>
        /// 读取翻译文件
        /// </summary>
        /// <returns></returns>
        public static string GetFileJson()
        {

            var ret = "";

            using var file = File.OpenText(FromUrl);

            using var reader = new JsonTextReader(file);
            var o = (JObject)JToken.ReadFrom(reader);

            var ageToken = o["texts"];

            if (ageToken != null)
            {
                ret = ageToken.ToString();
            }

            return ret;
        }





    }
}

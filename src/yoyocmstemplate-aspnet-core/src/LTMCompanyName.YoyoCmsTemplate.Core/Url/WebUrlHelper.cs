using System;
using System.Text.RegularExpressions;
using Abp.Extensions;
using LTMCompanyName.YoyoCmsTemplate.Configuration;

namespace LTMCompanyName.YoyoCmsTemplate.Url
{
    /// <summary>
    /// 自己扩展的urlhelper信息
    /// </summary>
    public static class WebUrlHelper
    {
        private static readonly Regex UrlWithProtocolRegex = new Regex("^.{1,10}://.*$");
        /// <summary>
        /// 公开站点地址
        /// </summary>
        public const string WebSiteClientRootAddressKey = "App:WebSiteClientRootAddress";


        public const string MarkdownPostSercerCode = "MarkdownPosts:SercertCode";
        /// <summary>
        /// 判断是否为根目录路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsRooted(string url)
        {
            return url.StartsWith("/") || WebUrlHelper.UrlWithProtocolRegex.IsMatch(url);
        }



        /// <summary>
        /// 获取公开站点的RootUrl地址
        /// </summary>
        /// <param name="appConfigurationAccessor">配置访问器</param>
        /// <returns></returns>
        public static string GetWebSiteClientRootAddress(this IAppConfigurationAccessor appConfigurationAccessor)
        {
            var url = appConfigurationAccessor.Configuration[WebSiteClientRootAddressKey];
            return url.EndsWith("/") ? url : $"{url}/";
        }


        /// <summary>
        /// 获取markdown的配置文件
        /// </summary>
        /// <param name="appConfigurationAccessor"></param>
        /// <returns></returns>
        public static string GetMarkdownPostSercerCode(this IAppConfigurationAccessor appConfigurationAccessor){

            var sercertCode = appConfigurationAccessor.Configuration[MarkdownPostSercerCode];


            return sercertCode;

         }





        /// <summary>
        /// 是否为外部链接
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsExternalLink(string path)
        {
            if (path.IsNullOrEmpty())
            {
                return false;
            }

            return path.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                   path.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
        }


    }




}

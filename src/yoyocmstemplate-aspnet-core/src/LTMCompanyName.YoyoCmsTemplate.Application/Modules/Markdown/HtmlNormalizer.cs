using System.Text.RegularExpressions;
using Abp.Extensions;
using LTMCompanyName.YoyoCmsTemplate.Url;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Markdown
{
    public static class HtmlNormalizer
    {
        /// <summary>
        /// 替换图片的源路径
        /// </summary>
        /// <param name="content"></param>
        /// <param name="documentRawRootUrl"></param>
        /// <param name="localDirectory"></param>
        /// <returns></returns>
        public static string ReplaceImageSources(string content, string documentRawRootUrl, string localDirectory)
        {
            if (content == null)
            {
                return null;
            }

            content = Regex.Replace(content, @"(<img\s+[^>]*)src=""([^""]*)""([^>]*>)", delegate (Match match)
                {
                    if (WebUrlHelper.IsExternalLink(match.Groups[2].Value))
                    {
                        return match.Value;
                    }

                    //本地的图片，需要上传到图床中去

                    var newImageSource = documentRawRootUrl.EnsureEndsWith('/') +
                                         (localDirectory.IsNullOrEmpty() ? "" : localDirectory.TrimStart('/').EnsureEndsWith('/')) +
                                         match.Groups[2].Value.TrimStart('/');

                    var url = match.Groups[1] + " src=\"" + newImageSource + "\" " + match.Groups[3];
                    return url;
                }, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

            return content;
        }

        /// <summary>
        /// 替换代码样式，为 prismJS 进行渲染准备，可以通过这里启动更多内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="currentLanguage">旧样式名称</param>
        /// <param name="newLanguage">新样式名称</param>
        /// <returns></returns>
        public static string ReplaceCodeBlocksLanguage(string content, string currentLanguage, string newLanguage)
        {

           //content= Regex.Replace(content, "<code>", "<code class=\"" + newLanguage + "\">", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);



      content= Regex.Replace(content, "<code class=\"" + currentLanguage + "\">", "<code class=\"" + newLanguage + "\">", RegexOptions.IgnoreCase);

            return content;
        }

        /// <summary>
        /// 为每个A标签 增加target=""_blank""
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string ReplaceCodeLinkUrl(string content)
        {
            //var content1 =  Regex.Matches(content, "href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);
            var linkRx = @"<a\s+(?:[^>]*?\s+)?href=([""])(.*?)\1";
            return Regex.Replace(content, linkRx, (Match match) =>
            {
                var oldUrl = match.Value;
                //aspnet-core-responsive-scaling-problem.md
                //yoyomooc/aspnet-core-responsive-scaling-problem

                if (oldUrl.Contains(".md"))
                {
                    oldUrl = oldUrl.Replace(".md", "");
                }

                //  oldUrl= oldUrl.Replace(".md", "");

                //yoyomooc/understanding-docker-and-container

                //<a href="understanding-docker-and-container.md"

                //todo:需要它的路由名称拼接而成

                //<a href="understanding-docker-and-container.md"       target="_blank"

                var newUrl = oldUrl + @"       target=""_blank""";
                return newUrl;
            }, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);
        }
    }
}

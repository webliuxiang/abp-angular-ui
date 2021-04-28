using System;
using System.Text;
using System.Text.RegularExpressions;
using Abp.Dependency;
using Abp.Extensions;
using L._52ABP.Application.GitlabAPIs;
using LTMCompanyName.YoyoCmsTemplate.Url;
using Markdig;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Markdown
{
    public class MarkDigMarkdownConverter : IMarkdownConverter, ISingletonDependency
    {
        readonly MarkdownPipeline _markdownPipeline;

     


        public const string Type = "md";

        public MarkDigMarkdownConverter()
        {
            _markdownPipeline = new MarkdownPipelineBuilder()
              .UseAdvancedExtensions()
              .Build();
         
        }

        public virtual string ConvertToHtml(string markdown)
        {

          //markdown=  NormalizeLinks(markdown);

          var content= Markdig.Markdown.ToHtml(Encoding.UTF8.GetString(Encoding.Default.GetBytes(markdown)),
                _markdownPipeline);

        //  content=  UploadpicturesToPictureBed(content);

            return content;


        }



       





        private string MdLinkFormat = $"[{{0}}](/{Type}/{{1}}/{{2}}{{3}}/{{4}})";
        private const string MarkdownLinkRegExp = @"\[(.*)\]\((.*\.md)\)";
        private const string AnchorLinkRegExp = @"<a[^>]+href=\""(.*?)\""[^>]*>(.*)?</a>";



        /// <summary>
        /// 对URL路径进行替换和重组
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>

        protected virtual string NormalizeLinks(
            string content
           
          
           )
        {
            var normalized = Regex.Replace(content, MarkdownLinkRegExp, delegate (Match match)
            {
                var link = match.Groups[2].Value;
                if (WebUrlHelper.IsExternalLink(link))
                {
                    return match.Value;
                }

                var displayText = match.Groups[1].Value;

                var documentName = RemoveFileExtension(link);
             

              

            var result= string.Format(MdLinkFormat,
                    displayText,                 
                    documentName
                );


                return result;


            });

            normalized = Regex.Replace(normalized, AnchorLinkRegExp, delegate (Match match)
            {
                var link = match.Groups[1].Value;
                if (WebUrlHelper.IsExternalLink(link))
                {
                    return match.Value;
                }

                var displayText = match.Groups[2].Value;
                var documentName = RemoveFileExtension(link);
           

                return string.Format(
                    MdLinkFormat,
                    displayText,                
                    documentName
                );
            });

            return normalized;
        }

        private static string RemoveFileExtension(string documentName)
        {
            if (documentName == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(documentName))
            {
                return documentName;
            }

            if (!documentName.EndsWith(Type, StringComparison.OrdinalIgnoreCase))
            {
                return documentName;
            }

            return documentName.Left(documentName.Length - Type.Length - 1);
        }






    }
}

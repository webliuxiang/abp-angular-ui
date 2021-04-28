using System.Linq;
using Abp.Extensions;

namespace LTMCompanyName.YoyoCmsTemplate.NamespaceHelper
{
    public class NamespaceHelperManager : YoyoCmsTemplateDomainServiceBase
    {

        /// <summary>
        /// 获取命名空间
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public string SplitNameSpace(string serviceName)
        {
            if (serviceName.IsNullOrEmpty() || !serviceName.Contains("."))
            {
                return serviceName;
            }

            if (serviceName.Contains("["))
            {
                return SplitGenericNamespace(serviceName);
            }

            return GetTextAfterLastDot(serviceName);
        }

        /// <summary>
        /// 获取文本中的一个点的位置
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string GetTextAfterLastDot(string text)
        {
            var lastDotIndex = text.LastIndexOf('.');
            return text.Substring(lastDotIndex + 1);
        }

        /// <summary>
        /// 拆解出类的命名空间
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        private static string SplitGenericNamespace(string serviceName)
        {
            var serviceNameParts = serviceName.Split('[').Where(s => !s.IsNullOrEmpty()).ToList();
            var genericServiceName = "";
            var openBracketCount = 0;

            for (var i = 0; i < serviceNameParts.Count; i++)
            {
                var serviceNamePart = serviceNameParts[i];
                if (serviceNamePart.Contains("`"))
                {
                    genericServiceName += GetTextAfterLastDot(serviceNamePart.Substring(0, serviceNamePart.IndexOf('`'))) + "<";
                    openBracketCount++;
                }

                if (serviceNamePart.Contains(","))
                {
                    genericServiceName += GetTextAfterLastDot(serviceNamePart.Substring(0, serviceNamePart.IndexOf(',')));
                    if (i + 1 < serviceNameParts.Count && serviceNameParts[i + 1].Contains(","))
                    {
                        genericServiceName += ", ";
                    }
                    else
                    {
                        genericServiceName += ">";
                        openBracketCount--;
                    }
                }
            }

            for (int i = 0; i < openBracketCount; i++)
            {
                genericServiceName += ">";
            }

            return genericServiceName;
        }
    }
}
using System;
using System.Collections.Concurrent;
using System.Text;
using Abp.IO.Extensions;
using Abp.Reflection.Extensions;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.Emailing;

namespace LTMCompanyName.YoyoCmsTemplate.Notifications.MailManage.Emailing
{
    public class EmailTemplateProvider : YoyoCmsTemplateDomainServiceBase, IEmailTemplateProvider
    {

        private readonly ConcurrentDictionary<string, string> _defaultTemplates;

        public EmailTemplateProvider()
        {

            _defaultTemplates = new ConcurrentDictionary<string, string>();
        }

        /// <summary>
        /// 获取默认的模板代码
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public string GetDefaultTemplate(int? tenantId)
        {
            var tenancyKey = tenantId.HasValue ? tenantId.Value.ToString() : "host";

            var templateStr = _defaultTemplates.GetOrAdd(tenancyKey, key =>
        {
            var stream = typeof(EmailTemplateProvider).GetAssembly()
                .GetManifestResourceStream("LTMCompanyName.YoyoCmsTemplate.Notifications.MailManage.Emailing.EmailTemplates.default.html");


            if (stream == null)
            {
                throw new UserFriendlyException("模板地址不存在");
            }


            using (stream)
            {
                var bytes = stream.GetAllBytes();
                var template = Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
                template = template.Replace("{THIS_YEAR}", DateTime.Now.Year.ToString());
                return template.Replace("{EMAIL_LOGO_URL}", GetTenantLogoUrl(tenantId));
            }
        });


            return templateStr;
        }

        private string GetTenantLogoUrl(int? tenantId)
        {
            //todo:待完善

            return "dd";
            //if (!tenantId.HasValue)
            //{
            //    return WebUrlHelper.GetAdminServerRootAddress().EnsureEndsWith('/') + "TenantCustomization/GetTenantLogo?skin=light";
            //}

            //return WebUrlHelper.WebSiteClientRootAddressKey().EnsureEndsWith('/') + "TenantCustomization/GetTenantLogo?skin=light&tenantId=" + tenantId.Value;
        }
    }
}

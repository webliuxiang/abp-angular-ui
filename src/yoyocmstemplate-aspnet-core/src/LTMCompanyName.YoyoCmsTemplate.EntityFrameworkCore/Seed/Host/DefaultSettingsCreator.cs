using System.Linq;
using Abp.Configuration;
using Abp.Localization;
using Abp.Net.Mail;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Seed.Host
{
    public class DefaultSettingsCreator
    {
        private readonly YoyoCmsTemplateDbContext _context;

        public DefaultSettingsCreator(YoyoCmsTemplateDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            // Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "ltm@ddxc.org");
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "52abp.com mailer");

            // Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, AppConsts.DefaultLanguage);
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.IgnoreQueryFilters()
                .Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null)) return;

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}
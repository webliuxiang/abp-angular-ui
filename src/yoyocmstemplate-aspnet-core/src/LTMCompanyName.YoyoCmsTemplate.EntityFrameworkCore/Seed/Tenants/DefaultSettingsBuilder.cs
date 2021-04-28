using System.Linq;
using Abp.Configuration;
using Abp.Localization;
using Abp.Net.Mail;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Seed.Tenants
{
    public class DefaultSettingsBuilder
    {
        private readonly YoyoCmsTemplateDbContext _context;
        private readonly int _tenantId;

        public DefaultSettingsBuilder(YoyoCmsTemplateDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        { // Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "info@ddxc.org", _tenantId);
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "52abp.com mailer", _tenantId);

            // Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, AppConsts.DefaultLanguage, _tenantId);
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

using System.Collections.Generic;
using System.Linq;
using Abp.Localization;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Seed.Host
{
    public class DefaultLanguagesCreator
    {
        private readonly YoyoCmsTemplateDbContext _context;

        public DefaultLanguagesCreator(YoyoCmsTemplateDbContext context)
        {
            _context = context;
        }

        public static List<ApplicationLanguage> InitialLanguages => GetInitialLanguages();

        private static List<ApplicationLanguage> GetInitialLanguages()
        {
            return new List<ApplicationLanguage>
            {
                new ApplicationLanguage(null, "en", "English", "famfamfam-flags us"),

                new ApplicationLanguage(null, AppConsts.DefaultLanguage, "简体中文", "famfamfam-flags cn"),
            };
        }

        public void Create()
        {
            CreateLanguages();
        }

        private void CreateLanguages()
        {
            foreach (var language in InitialLanguages) AddLanguageIfNotExists(language);
        }

        private void AddLanguageIfNotExists(ApplicationLanguage language)
        {
            if (_context.Languages.IgnoreQueryFilters()
                .Any(l => l.TenantId == language.TenantId && l.Name == language.Name)) return;

            _context.Languages.Add(language);
            _context.SaveChanges();
        }
    }
}
using System.Linq;
using Abp.Application.Features;
using LTMCompanyName.YoyoCmsTemplate.Editions;
using LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.Features;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Seed.Host
{
    public class DefaultEditionCreator
    {
        private readonly YoyoCmsTemplateDbContext _context;

        public DefaultEditionCreator(YoyoCmsTemplateDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateEditions();
        }

        private void CreateEditions()
        {
            var defaultEdition = _context.Editions.IgnoreQueryFilters()
                .FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);

            if (defaultEdition == null)
            {
                defaultEdition = new SubscribableEdition
                {
                    Name = EditionManager.DefaultEditionName,
                    DisplayName = EditionManager.DefaultEditionName
                };
                _context.Editions.Add(defaultEdition);
                _context.SaveChanges();

                /* Add desired features to the standard edition, if wanted... */
            }

            if (defaultEdition.Id > 0)
            {
                CreateFeatureIfNotExists(defaultEdition.Id, AppFeatures.ChatFeature, true);
                CreateFeatureIfNotExists(defaultEdition.Id, AppFeatures.TenantToTenantChatFeature, true);
                CreateFeatureIfNotExists(defaultEdition.Id, AppFeatures.TenantToHostChatFeature, true);
            }
        }

        private void CreateFeatureIfNotExists(int editionId, string featureName, bool isEnabled)
        {
            var defaultEditionChatFeature = _context.EditionFeatureSettings.IgnoreQueryFilters()
                .FirstOrDefault(ef => ef.EditionId == editionId && ef.Name == featureName);

            if (defaultEditionChatFeature == null)
            {
                _context.EditionFeatureSettings.Add(new EditionFeatureSetting
                {
                    Name = featureName,
                    Value = isEnabled.ToString().ToLower(),
                    EditionId = editionId
                });
            }



        }
    }
}
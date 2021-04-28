using Abp.Domain.Services;

namespace LTMCompanyName.YoyoCmsTemplate.Emailing
{
    public interface IEmailTemplateProvider : IDomainService
    {
        string GetDefaultTemplate(int? tenantId);
    }
}

using System.Threading.Tasks;
using Abp.Application.Services;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Documents.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Documents
{
    public interface IDocumentAppService : IApplicationService
    {
        Task<DocumentWithDetailsDto> GetDocDetailsAsync(GetDocumentInput input);

        Task<DocumentWithDetailsDto> GetNavigationDocumentAsync(GetNavigationDocumentInput input);

        

    }
}

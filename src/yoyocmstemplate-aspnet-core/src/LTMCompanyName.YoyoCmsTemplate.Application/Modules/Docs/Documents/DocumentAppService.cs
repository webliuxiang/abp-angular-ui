using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Documents.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects;
using LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Documents.Models;
using YoYo.ABPCommunity.Docs.Documents.Services;
using YoYo.ABPCommunity.Docs.Projects.DomainService;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Documents
{
    public class DocumentAppService : ApplicationService, IDocumentAppService
    {
        private readonly IRepository<Project, Guid> _projectRepository;


        private readonly IDocumentStoreFactory _documentStoreFactory;

        private readonly ICacheManager _cacheManager;

        public DocumentAppService(
            IRepository<Project, Guid> projectRepository,
            IDocumentStoreFactory documentStoreFactory,
            ICacheManager cacheManager
            )
        {
            _projectRepository = projectRepository;
            _documentStoreFactory = documentStoreFactory;
            _cacheManager = cacheManager;
        }

        public async Task<DocumentWithDetailsDto> GetDocDetailsAsync(GetDocumentInput input)
        {

            var project = await _projectRepository.GetAsync(input.ProjectId);

            
         
                var dto = await GetDocumentWithDetailsDto(project, input.Name, input.Version);

                return dto;
         

          


        }

        public async Task<DocumentWithDetailsDto> GetNavigationDocumentAsync(GetNavigationDocumentInput input)
        {

            var project = await _projectRepository.GetAsync(input.ProjectId);
            var dto = await GetDocumentWithDetailsDto(
                project,
                project.NavigationDocumentName,
                input.Version
            );

            return dto;
            




        }

    

        protected virtual async Task<DocumentWithDetailsDto> GetDocumentWithDetailsDto(
            Project project,
            string documentName,
            string version)
        {
            var cacheKey = $"Document@{project.ShortName}#{documentName}#{version}";


            Document document;

            async Task<Document> GetDocumentAsync()
            {
                Logger.Info($"?????????????????????????????????????????? {documentName}...");
                var store = _documentStoreFactory.Create(project.DocumentStoreType);
                //?????????Github?????????Filesystem????????????????????????
               document = await store.GetDocumentAsync(project, documentName, version);
                Logger.Info($"????????????: {documentName}");
                return document;
            }
          
            //?????????Debug????????????????????????
            if (Debugger.IsAttached)
            {
                //????????????
                document = await GetDocumentAsync();
            }
            var documentCache = _cacheManager.GetCache(cacheKey).AsTyped<string, Document>();

           document = await documentCache.GetOrDefaultAsync(cacheKey);

            if (document == null)
            {
              var cacheDocument = await GetDocumentAsync();
               
                _cacheManager.GetCache(cacheKey).Set(cacheKey, cacheDocument, null, TimeSpan.FromHours(30));
            }

            //??????????????????????????????

            var documentDto = new DocumentWithDetailsDto();
            ObjectMapper.Map(document, documentDto);


            
            return documentDto;


          
 



      
        }












    }
}

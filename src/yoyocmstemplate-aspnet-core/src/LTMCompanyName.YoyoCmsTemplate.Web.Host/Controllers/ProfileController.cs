using Abp.AspNetCore.Mvc.Authorization;
using LTMCompanyName.YoyoCmsTemplate.AppFolders;
using LTMCompanyName.YoyoCmsTemplate.Controllers;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects.DataTempCache;
using LTMCompanyName.YoyoCmsTemplate.SystemBaseManage;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Host.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : UploadFilesControllerBase
    {
        private readonly IAppFolder _appFolder;
        private readonly ISysFileAppService _sysFileAppService;

        public ProfileController(IAppFolder appFolder, IDataFileObjectManager dataFileObjectManager, IDataTempFileCacheManager dataTempFileCacheManager, ISysFileAppService sysFileAppService) : base(appFolder, dataFileObjectManager, dataTempFileCacheManager)
        {
            _appFolder = appFolder;
            _sysFileAppService = sysFileAppService;
        }
    }
}

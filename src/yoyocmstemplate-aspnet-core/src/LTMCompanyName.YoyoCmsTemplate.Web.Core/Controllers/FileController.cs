using System.IO;
using Abp.Auditing;
using Abp.UI;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.AppFolders;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects.DataTempCache;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Controllers
{
    /// <summary>
    /// 文件控制器
    /// </summary>
    public class FileController : YoyoCmsTemplateControllerBase
    {
        private readonly IAppFolder _appFolders;
        private readonly IDataTempFileCacheManager _dataTempFileCacheManager;

        public FileController(IAppFolder appFolders, IDataTempFileCacheManager dataTempFileCacheManager)
        {
            _appFolders = appFolders;
            _dataTempFileCacheManager = dataTempFileCacheManager;
        }

        /// <summary>
        /// 根据文件token下载文件
        /// </summary>
        /// <param name="file"> </param>
        /// <returns> </returns>

        [DisableAuditing]
        public ActionResult DownloadFilePathFile(FileDto file)
        {
            var filePath = Path.Combine(_appFolders.TempFileDownloadFolder, file.FileToken);
            if (!System.IO.File.Exists(filePath))
            {
                throw new UserFriendlyException(L("RequestedFileDoesNotExists"));
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            System.IO.File.Delete(filePath);
            return File(fileBytes, file.FileType, file.FileName);
        }

        /// <summary>
        /// 根据文件token下载文件
        /// </summary>
        /// <param name="file"> </param>
        /// <returns> </returns>
        [DisableAuditing]
        public ActionResult DownloadTempFile(FileDto file)
        {
            var fileBytes = _dataTempFileCacheManager.GetFile(file.FileToken);

            if (fileBytes == null)
            {
                return NotFound("当前的文件信息不存在");
            }

            return File(fileBytes, file.FileType, file.FileName);
        }
    }
}

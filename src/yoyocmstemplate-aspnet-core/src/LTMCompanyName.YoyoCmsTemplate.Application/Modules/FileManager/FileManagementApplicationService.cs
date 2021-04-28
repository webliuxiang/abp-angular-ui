using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.IO.Extensions;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects.DataTempCache;
using LTMCompanyName.YoyoCmsTemplate.SystemBaseManage;
using LTMCompanyName.YoyoCmsTemplate.SystemBaseManage.DomainService;
using LTMCompanyName.YoyoCmsTemplate.SystemBaseManage.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.FileManager
{
    /// <summary>
    ///     文件管理服务类
    /// </summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class FileManagementApplicationService : YoyoCmsTemplateAppServiceBase
    {
        private readonly IDataFileObjectManager _dataFileObjectManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataTempFileCacheManager _tempFileCacheManager;

        private readonly ISysFileManager _sysFileManager;


        public FileManagementApplicationService(IHttpContextAccessor httpContextAccessor,
            IDataFileObjectManager dataFileObjectManager, IDataTempFileCacheManager tempFileCacheManager, ISysFileManager sysFileManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _dataFileObjectManager = dataFileObjectManager;
            _tempFileCacheManager = tempFileCacheManager;
            _sysFileManager = sysFileManager;
        }

        /// <summary>
        ///     上传文件到数据库中
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<FileUploadOutputDto> UploadFileToData(IFormFile file)
        {
            //Check input
            if (file == null)
            {
                throw new Exception("File empty");
            }

            if (file.Length > AppConsts.MaxFileSize)
            {
                throw new Exception("File cannot be larger than 50mb");
            }

            byte[] fileBytes;
            using (var stream = file.OpenReadStream())
            {
                fileBytes = stream.GetAllBytes();
            }

            var tenantId = AbpSession.TenantId;
            var fileObject = new DataFileObject(tenantId, fileBytes);

            await _dataFileObjectManager.SaveAsync(fileObject);

            return new FileUploadOutputDto
            {
                FileName = file.FileName,
                FileToken = fileObject.Id.ToString(),
                FileSize = file.Length
            };
        }


        /// <summary>
        /// 上传文件到文件夹
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<List<SysFileListDto>> UploadFileToSysFiles(IFormFile file)
        {

            var upfiles = _httpContextAccessor.HttpContext.Request.Form.Files;

            //这样会产生脏数据。。。。。目前的逻辑。
            var dtos = new List<SysFileListDto>();


            //根据Code节点来保存

            if (upfiles != null && upfiles.Count > 0)
            {
                foreach (var item in upfiles)
                {
                    var entity = await _sysFileManager.ProcessUploadedFileAsync(item, true);

                    //调用领域服务
                    await _sysFileManager.CreateAsync(entity);

                    var dto = ObjectMapper.Map<SysFileListDto>(entity);

                    dtos.Add(dto);

                }
            }
            //应该在信息处理完成后 将数据移动到另外一个正式文件夹。
            //这块需要探讨下。
            return dtos;

        }



        /// <summary>
        ///     上传文件到缓存中
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<FileUploadOutputDto> UploadTempFile(IFormFile file)
        {
            await Task.Yield();
            //Check input
            if (file == null)
            {
                throw new Exception("File empty");
            }

            if (file.Length > AppConsts.MaxFileSize)
            {
                throw new Exception("File cannot be larger than 50mb");
            }

            byte[] fileBytes;
            using (var stream = file.OpenReadStream())
            {
                fileBytes = stream.GetAllBytes();
            }

            var tenantId = AbpSession.TenantId;

            var storedFile = new DataFileObject(tenantId, fileBytes.ToArray());
            _tempFileCacheManager.SetFile(storedFile.Id.ToString(), fileBytes.ToArray());

            return new FileUploadOutputDto
            {
                FileName = file.FileName,
                FileToken = storedFile.Id.ToString(),
                FileSize = file.Length
            };
        }

        ///// <summary>
        /////     下载文件
        ///// </summary>
        ///// <param name="fileId"></param>
        ///// <param name="fileName"></param>
        ///// <returns></returns>
        //[ProducesResponseType(typeof(FileContentResult), 200)]
        //[AllowAnonymous]
        //[HttpGet]
        //[HttpPost]
        //public async Task<IActionResult> DownloadFile(Guid fileId, string fileName = null)
        //{
        //    var fileIdstr = fileId.ToString();

        //    var file = _tempFileCacheManager.GetDataFileObject(fileIdstr) ??
        //               await _dataFileObjectManager.GetOrNullAsync(fileId);

        //    var provider = new FileExtensionContentTypeProvider();
        //    fileName = fileName ?? Guid.NewGuid().ToString();
        //    if (!provider.TryGetContentType(fileName, out var contentType))
        //    {
        //        contentType = "application/octet-stream";
        //    }

        //    var fileResult = new FileContentResult(file.Bytes, contentType) {FileDownloadName = fileName};
        //    return fileResult;
        //}
    }

    /// <summary>
    ///     文件上传输出Dto
    /// </summary>
    public class FileUploadOutputDto
    {
        [Required] public string FileName { get; set; }

        [Required] public string FileToken { get; set; }

        [Required] public long FileSize { get; set; }
    }
}

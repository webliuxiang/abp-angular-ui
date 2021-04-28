using System;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Auditing;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using L._52ABP.Application.Dtos;
using L._52ABP.Common.Helpers;
using L._52ABP.Common.Net.MimeTypes;
using LTMCompanyName.YoyoCmsTemplate.AppFolders;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects.DataTempCache;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.UploadFiles.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Controllers
{
    /// <summary>
    /// 上传文件的控制器基类
    /// </summary>

    [DisableAuditing]
    public abstract class UploadFilesControllerBase : YoyoCmsTemplateControllerBase
    {
        private readonly IAppFolder _appFolder;
        private const int MaxProfilePictureSize = 5242880; //5MB

        private readonly IDataFileObjectManager _dataFileObjectManager;
        private readonly IDataTempFileCacheManager _dataTempFileCacheManager;

        protected UploadFilesControllerBase(
        IAppFolder appFolder,
        IDataFileObjectManager dataFileObjectManager,
        IDataTempFileCacheManager dataTempFileCacheManager
         )
        {
            _appFolder = appFolder;
            _dataFileObjectManager = dataFileObjectManager;
            _dataTempFileCacheManager = dataTempFileCacheManager;
            LocalizationSourceName = AppConsts.LocalizationSourceName;
        }

        /// <summary>
        /// 上传头像的输出
        /// </summary>
        /// <returns> </returns>
        [AbpMvcAuthorize]
        public UploadProfilePictureOutputDto UploadProfilePicture(FileDto input)
        {
            try
            {
                var profilePictureFile = Request.Form.Files.First();

                //Check input
                if (profilePictureFile == null)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }

                //上传的图片超过大小限制
                if (profilePictureFile.Length > MaxProfilePictureSize)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit", AppConsts.MaxProfilePictureBytesUserFriendlyValue));
                }

                byte[] fileBytes;
                using (var stream = profilePictureFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                if (!ImageFormatHelper.GetRawImageFormat(fileBytes).IsIn(ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif))
                {
                    throw new Exception("请上传图片文件，仅接受Jpg、PNG、Gif三种格式!");
                }

                _dataTempFileCacheManager.SetFile(input.FileToken, fileBytes);

                using (var bmpImage = new Bitmap(new MemoryStream(fileBytes)))
                {
                    return new UploadProfilePictureOutputDto
                    {
                        FileToken = input.FileToken,
                        FileName = input.FileName,
                        FileType = input.FileType,
                        Width = bmpImage.Width,
                        Height = bmpImage.Height
                    };
                }
            }
            catch (UserFriendlyException ex)
            {
                return new UploadProfilePictureOutputDto(new ErrorInfo(ex.Message));
            }
        }

        /// <summary>
        /// 上传头像返回图片Id
        /// </summary>
        [AbpMvcAuthorize]
        public async Task<UploadProfilePictureOutputDto> UploadProfilePictureReturnFileId()
        {
            try
            {
                var profilePictureFile = Request.Form.Files.First();

                //Check input
                if (profilePictureFile == null)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }

                if (profilePictureFile.Length > MaxProfilePictureSize)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit", AppConsts.MaxProfilePictureBytesUserFriendlyValue));
                }

                byte[] fileBytes;
                using (var stream = profilePictureFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                if (!ImageFormatHelper.GetRawImageFormat(fileBytes).IsIn(ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif))
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Info", AppConsts.MaxProfilePictureBytesUserFriendlyValue));
                }

                var storedFile = new DataFileObject(AbpSession.TenantId, fileBytes.ToArray());
                await _dataFileObjectManager.SaveAsync(storedFile);

                return new UploadProfilePictureOutputDto
                {
                    FileName = profilePictureFile.FileName,
                    ProfilePictureId = storedFile.Id
                };
            }
            catch (UserFriendlyException ex)
            {
                return new UploadProfilePictureOutputDto(new ErrorInfo(ex.Message));
            }
        }



        protected virtual FileResult GetDefaultProfilePicture()
        {
            return File(
                @"Common\Images\default-52abp-picture.png",
                MimeTypeNames.ImagePng
            );
        }


    }
}

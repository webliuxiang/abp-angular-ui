using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Extensions;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.AppFolders;
using LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatMedias.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.Media;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatMedias
{

    [AbpAuthorize]
    public class WechatMediaAppService : YoyoCmsTemplateAppServiceBase, IWechatMediaAppService
    {
        private const string MediaRootFoler = "Uploads";

        private readonly IWechatAppConfigManager _wechatAppConfigManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppFolder _appFolder;

        public WechatMediaAppService(
             IWechatAppConfigManager wechatAppConfigManager,
             IHttpContextAccessor httpContextAccessor,
             IAppFolder appFolder
            )
        {
            _wechatAppConfigManager = wechatAppConfigManager;
            _httpContextAccessor = httpContextAccessor;
            _appFolder = appFolder;
        }

        [HttpPost]
        public async Task<PagedResultDto<MediaList_News_Item>> GetImageTextMaterialPaged(GetImageTextMaterialsInput input)
        {
            if (input.MaxResultCount > 20)
            {
                input.MaxResultCount = 20;
            }

            await _wechatAppConfigManager.RegisterWechatApp(input.AppId);

            var result = await Senparc.Weixin.MP.AdvancedAPIs.MediaApi.GetNewsMediaListAsync(input.AppId, input.SkipCount, input.MaxResultCount);

            if (!result.errmsg.IsNullOrWhiteSpace())
            {
                throw new UserFriendlyException(result.errmsg);
            }

            return new PagedResultDto<MediaList_News_Item>()
            {
                TotalCount = result.total_count,
                Items = ObjectMapper.Map<IReadOnlyList<MediaList_News_Item>>(result.item)
            };
        }


        [HttpPost]
        public async Task<PagedResultDto<MediaList_Others_Item>> GetOtherMaterialPaged(GetOtherMaterialsInput input)
        {
            if (input.MaxResultCount > 20)
            {
                input.MaxResultCount = 20;
            }

            await _wechatAppConfigManager.RegisterWechatApp(input.AppId);

            var result = await Senparc.Weixin.MP.AdvancedAPIs.MediaApi.GetOthersMediaListAsync(input.AppId, input.MaterialType, input.SkipCount, input.MaxResultCount);

            if (!result.errmsg.IsNullOrWhiteSpace())
            {
                throw new UserFriendlyException(result.errmsg);
            }

            //if (input.MaterialType == UploadMediaFileType.image && result.item != null && result.item.Count > 0)
            //{
            //    foreach (var item in result.item)
            //    {
            //        item.url = item.url.Replace("http://", "https://");
            //    }
            //}

            return new PagedResultDto<MediaList_Others_Item>()
            {
                TotalCount = result.total_count,
                Items = ObjectMapper.Map<IReadOnlyList<MediaList_Others_Item>>(result.item)
            };
        }

        public async Task Delete(string appId, string mediaId)
        {
            await Senparc.Weixin.MP.AdvancedAPIs.MediaApi.DeleteForeverMediaAsync(appId, mediaId);
        }


        [HttpPost]
        public async Task<string> CreateOtherrMaterial()
        {
            var filePath = string.Empty;
            try
            {
                var appId = this._httpContextAccessor.HttpContext.Request.Form["appId"];
                var mediaFileType = (UploadMediaFileType)Convert.ToInt32(this._httpContextAccessor.HttpContext.Request.Form["mediaFileType"]);


                await this._wechatAppConfigManager.RegisterWechatApp(appId);


                var file = this._httpContextAccessor.HttpContext.Request.Form.Files.First();
                filePath = this.SaveFile(file);


                var result = await Senparc.Weixin.MP.AdvancedAPIs.MediaApi.UploadForeverMediaAsync(appId, filePath);

                if (!result.errmsg.IsNullOrWhiteSpace())
                {
                    throw new UserFriendlyException(result.errmsg);
                }

                // 新增图片则返回图片地址
                if (mediaFileType == UploadMediaFileType.image)
                {
                    return result.url;
                    //return result.url.Replace("http://", "https://");
                }


                return string.Empty;

            }
            catch (UserFriendlyException e)
            {

                throw e;
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

        }


        #region 保存到本地

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns>保存后的路径</returns>
        private string SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            originalFileName = Senparc.CO2NET.Extensions.WebCodingExtensions.AsUrlData(originalFileName);

            var tenantId = "0";
            var userId = AbpSession.UserId.Value.ToString();
            if (AbpSession.TenantId.HasValue)
            {
                tenantId = AbpSession.TenantId.Value.ToString();
            }
            var dirPath = Path.Combine(_appFolder.WebContentRootPath, MediaRootFoler, tenantId, userId);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            var saveFullPath = Path.Combine(dirPath, originalFileName).Replace("\\", "/");

            using (var output = new FileStream(saveFullPath, FileMode.Create))
            {
                file.OpenReadStream().CopyTo(output);
            }

            return saveFullPath;
        }


        #endregion

    }
}

using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatMedias.Dtos;
using Senparc.Weixin.MP.AdvancedAPIs.Media;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatMedias
{
    public interface IWechatMediaAppService : IApplicationService
    {
        /// <summary>
        /// 分页获取图文素材
        /// </summary>
        /// <param name="input">分页数据</param>
        /// <returns></returns>
        Task<PagedResultDto<MediaList_News_Item>> GetImageTextMaterialPaged(GetImageTextMaterialsInput input);


        /// <summary>
        /// 分页获取图片、语音、视频素材
        /// </summary>
        /// <param name="input">分页数据</param>
        /// <returns></returns>
        Task<PagedResultDto<MediaList_Others_Item>> GetOtherMaterialPaged(GetOtherMaterialsInput input);


        /// <summary>
        /// 删除素材
        /// </summary>
        /// <param name="appId">appId</param>
        /// <param name="mediaId">素材Id</param>
        /// <returns></returns>
        Task Delete(string appId, string mediaId);

        /// <summary>
        /// 上传素材(素材数据在 Request.Form.Files 中)
        /// 
        /// appId与mediaFileType 都在 Request.Form 中
        /// </summary>
        /// <returns></returns>
        Task<string> CreateOtherrMaterial();
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

using Abp.Domain.Services;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources.DomainService
{
    public interface IVideoResourceManager : I52AbpDomainService<VideoResource>
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        string InitVideoResource();


        /// <summary>
        /// 获取最后一个视频
        /// </summary>
        /// <returns></returns>
        Task<VideoResource> GetLastVideoResource();


        /// <summary>
        /// 同步阿里云vod的视频数据
        /// </summary>
        /// <returns></returns>
        Task<List<VideoResource>> SynchronousVideoVodData();

        Task SychronousVideoInsertToData(List<VideoResource> newVideoInfos);


    }
}

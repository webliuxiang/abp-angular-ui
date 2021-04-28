using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

using LTMCompanyName.YoyoCmsTemplate.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources
{
    /// <summary>
    /// VideoResource应用层服务的接口方法
    ///</summary>
    public interface IVideoResourceAppService : IApplicationService
    {
        /// <summary>
		/// 获取VideoResource的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<VideoResourceListDto>> GetPaged(QueryInput input);


        /// <summary>
        /// 通过指定id获取VideoResourceListDto信息
        /// </summary>
        Task<VideoResourceListDto> GetById(EntityDto<long> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetVideoResourceForEditOutput> GetForEdit(NullableIdDto<long> input);


        /// <summary>
        /// 添加或者修改VideoResource的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateVideoResourceInput input);


        /// <summary>
        /// 删除VideoResource信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<string> input);


        /// <summary>
        /// 批量删除VideoResource
        /// </summary>
        Task BatchDelete(List<string> input);


        dynamic GetVodTest();


        /// <summary>
        /// 同步阿里云视频到本地数据
        /// </summary>
        /// <returns></returns>
        Task<dynamic> SynchronizeAliyunVodAsync();

        /// <summary>
        /// 获取视频播放的授权
        /// </summary>
        /// <param name="videoInput">视频id</param>
        /// <returns></returns>
        Task<VideoPlayAuthDto> GetVideoResourcePlayAuth(EntityDto<string> videoInput);


        /// <summary>
        /// 搜索视频资源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ListResultDto<VideoResourceListDto>> SearchVideoResources(SearchVideoResourceInput input);
    }
}

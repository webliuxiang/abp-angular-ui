using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.BannerAds.Dtos;



namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.BannerAds
{
    /// <summary>
    /// 轮播图广告应用层服务的接口方法
    ///</summary>
    public interface IBannerImgAppService : IApplicationService
    {
        /// <summary>
		/// 获取轮播图广告的分页列表集合
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<BannerAdListDto>> GetPaged(GetBannerAdsInput input);


        /// <summary>
        /// 通过指定id获取轮播图广告ListDto信息
        /// </summary>
        Task<BannerAdListDto> GetById(EntityDto<int> input);


        /// <summary>
        /// 返回实体轮播图广告的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetBannerAdForEditOutput> GetForEdit(NullableIdDto<int> input);


        /// <summary>
        /// 添加或者修改轮播图广告的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateBannerAdInput input);


        /// <summary>
        /// 删除轮播图广告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<int> input);


        /// <summary>
        /// 批量删除轮播图广告
        /// </summary>
        Task BatchDelete(List<int> input);



        //// custom codes
        /// <summary>
        /// 获取前端门户使用的banner图
        /// </summary>
        /// <returns></returns>
        Task<List<BannerAdListDto>> GetForReadBannerAds();

        //// custom codes end
    }
}

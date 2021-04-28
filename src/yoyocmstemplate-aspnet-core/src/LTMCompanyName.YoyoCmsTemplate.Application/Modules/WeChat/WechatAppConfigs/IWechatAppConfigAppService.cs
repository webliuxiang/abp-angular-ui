using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs
{
    /// <summary>
    /// WechatAppConfig应用层服务的接口方法
    ///</summary>
    public interface IWechatAppConfigAppService : IApplicationService
    {
        /// <summary>
		/// 获取WechatAppConfig的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<WechatAppConfigListDto>> GetPaged(GetWechatAppConfigsInput input);


        /// <summary>
        /// 通过指定id获取WechatAppConfigListDto信息
        /// </summary>
        Task<WechatAppConfigListDto> GetById(EntityDto<int> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetWechatAppConfigForEditOutput> GetForEdit(NullableIdDto<int> input);


        /// <summary>
        /// 添加或者修改WechatAppConfig的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateWechatAppConfigInput input);


        /// <summary>
        /// 删除WechatAppConfig信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<int> input);


        /// <summary>
        /// 批量删除WechatAppConfig
        /// </summary>
        Task BatchDelete(List<int> input);


        /// <summary>
        /// 将wechat app注入到容器,如果已注入则刷新注入
        /// </summary>
        /// <returns></returns>
        Task RegisterWechatApp(string appId);

        ///// <summary>
        ///// 导出WechatAppConfig为excel表
        ///// </summary>
        ///// <returns></returns>
        //Task<FileDto> GetToExcel();


    }
}

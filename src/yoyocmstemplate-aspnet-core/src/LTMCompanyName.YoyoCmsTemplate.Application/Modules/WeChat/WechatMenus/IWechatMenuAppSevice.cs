using System.Threading.Tasks;
using Abp.Application.Services;
using LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatMenus.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatMenus
{
    public interface IWechatMenuAppSevice : IApplicationService
    {
        /// <summary>
        /// 获取所有菜单
        /// (自定义菜单和所有个性化菜单)
        /// </summary>
        /// <param name="appId">微信AppId</param>
        /// <returns>菜单数据</returns>
        Task<GetWechatMenuForEditOutput> GetMenuForEdit(string appId);

        /// <summary>
        /// 编辑自定义菜单或创建个性化菜单的输入
        /// </summary>
        /// <param name="input">菜单信息</param>
        /// <returns></returns>
        Task CreateOrWechatEditMenu(CreateOrEditWechatMenuInput input);

        /// <summary>
        /// 删除一个个性化菜单
        /// </summary>
        /// <param name="appId">微信AppId</param>
        /// <param name="menuConditionalId">菜单id</param>
        /// <returns></returns>
        Task DeleteMenuConditional(string appId, string menuConditionalId);
    }
}

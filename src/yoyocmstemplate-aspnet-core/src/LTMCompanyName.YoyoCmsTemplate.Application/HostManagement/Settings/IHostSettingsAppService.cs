using System.Threading.Tasks;
using Abp.Application.Services;
using LTMCompanyName.YoyoCmsTemplate.Configuration.Dtos;
using LTMCompanyName.YoyoCmsTemplate.HostManagement.Settings.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.HostManagement.Settings
{
    public interface IHostSettingsAppService : IApplicationService
    {
        /// <summary>
        /// 获取所有设置
        /// </summary>
        /// <returns></returns>
        Task<HostSettingsEditDto> GetAllSettings();

        /// <summary>
        /// 更新所有设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAllSettings(HostSettingsEditDto input);

        /// <summary>
        /// 发送测试邮件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SendTestEmail(SendTestEmailInput input);


    }
}

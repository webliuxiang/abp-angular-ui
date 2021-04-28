using Abp.Application.Services;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.AbpProject
{
    /// <summary>
    ///     ABP模板项目生成服务
    /// </summary>
    public interface IAbpProjectAppService : IApplicationService
    {
        /// <summary>
        ///     生成ABP项目的ZIP文件
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="filePath"></param>
        void GenerateAbpProjectZip(string dirPath, string filePath);
    }
}

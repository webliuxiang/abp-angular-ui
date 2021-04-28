using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Sessions.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Session
{
    public interface IWebSessionCache
    {
        /// <summary>
        /// 将当前登录用户信息写入到缓存中
        /// </summary>
        /// <returns> </returns>
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
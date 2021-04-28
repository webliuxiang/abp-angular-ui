using System.Threading.Tasks;
using Abp.Dependency;
using LTMCompanyName.YoyoCmsTemplate.Sessions;
using LTMCompanyName.YoyoCmsTemplate.Sessions.Dto;
using Microsoft.AspNetCore.Http;

namespace LTMCompanyName.YoyoCmsTemplate.Session
{
    public class WebSessionCache : IWebSessionCache, ITransientDependency
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionAppService _sessionAppService;

        public WebSessionCache(
            IHttpContextAccessor httpContextAccessor,
            ISessionAppService sessionAppService)
        {
            _httpContextAccessor = httpContextAccessor;
            _sessionAppService = sessionAppService;
        }

        /// <summary>
        /// 将当前登录用户信息写入到缓存中
        /// </summary>
        /// <returns> </returns>
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return await _sessionAppService.GetCurrentLoginInformations();
            }

            var cachedValue = httpContext.Items["__WebSessionCache"] as GetCurrentLoginInformationsOutput;
            if (cachedValue == null)
            {
                cachedValue = await _sessionAppService.GetCurrentLoginInformations();
                httpContext.Items["__WebSessionCache"] = cachedValue;
            }

            return cachedValue;
        }
    }
}
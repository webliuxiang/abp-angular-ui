using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Abp;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.Threading;
using L._52ABP.Common.Consts;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.IdentityModel.Tokens;

namespace LTMCompanyName.YoyoCmsTemplate.Authentication.JwtBearer
{
    public class YoYoJwtSecurityTokenHandler : ISecurityTokenValidator
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public YoYoJwtSecurityTokenHandler()
        {
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; } = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;

        public bool CanReadToken(string securityToken)
        {
            return _tokenHandler.CanReadToken(securityToken);
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            var cacheManager = IocManager.Instance.Resolve<ICacheManager>();

            // 解析验证当前请求的token
            var principal = _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);

            // 从解析的token中获取用户id和token校验值
            var userIdentifierString = principal.Claims.First(c => c.Type == AbpProConsts.UserIdentifier);
            var tokenValidityKeyInClaims = principal.Claims.First(c => c.Type == AbpProConsts.TokenValidityKey);

            // 从缓存中获取是否存在以token校验值作为键的缓存
            var tokenValidityKeyInCache = cacheManager
                .GetCache(AbpProConsts.TokenValidityKey)
                .GetOrDefault(tokenValidityKeyInClaims.Value);

            // 缓存不为空则表示验证通过
            if (tokenValidityKeyInCache != null)
            {
                return principal;
            }

            // 缓存为空则从用户关联的 UserToken 表中查询出
            // LoginProvider 值为 TokenValidityKeyProvider 并且 Name 值为 tokenValidityKeyInClaims.Value
            // 的token，判断是否符合校验条件
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = unitOfWorkManager.Object.Begin())
                {
                    var userIdentifier = UserIdentifier.Parse(userIdentifierString.Value);

                    using (unitOfWorkManager.Object.Current.SetTenantId(userIdentifier.TenantId))
                    {
                        using (var userManager = IocManager.Instance.ResolveAsDisposable<UserManager>())
                        {
                            var userManagerObject = userManager.Object;
                            var user = userManagerObject.GetUser(userIdentifier);
                            var isValidityKetValid = AsyncHelper.RunSync(() =>
                                userManagerObject.IsTokenValidityKeyValidAsync(user, tokenValidityKeyInClaims.Value)
                            );
                            uow.Complete();

                            // 符合校验条件加入缓存,下次访问走缓存速度更快
                            if (isValidityKetValid)
                            {
                                cacheManager
                                    .GetCache(AbpProConsts.TokenValidityKey)
                                    .Set(tokenValidityKeyInClaims.Value, "");

                                return principal;
                            }
                        }
                    }

                    // 校验失败,抛出校验token失败异常
                    throw new SecurityTokenException("invalid");
                }
            }
        }
    }
}

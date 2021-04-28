using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Json;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Abp.UI;
using L._52ABP.Common.Consts;
using L._52ABP.Common.Net.MimeTypes;
using L._52ABP.Core.VerificationCodeStore;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.Dto;
using LTMCompanyName.YoyoCmsTemplate.Authentication.JwtBearer;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Impersonation;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects.DataTempCache;
using LTMCompanyName.YoyoCmsTemplate.Models.TokenAuth;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users.UserLink;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LTMCompanyName.YoyoCmsTemplate.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TokenAuthController : YoyoCmsTemplateControllerBase
    {
        private readonly LogInManager _logInManager;
        private readonly ITenantCache _tenantCache;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly TokenAuthConfiguration _configuration;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;
        private readonly IExternalAuthManager _externalAuthManager;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly ICacheManager _cacheManager;
        private readonly IImpersonationManager _impersonationManager;
        private readonly IUserLinkManager _userLinkManager;
        private readonly IdentityOptions _identityOptions;
        private readonly UserManager _userManager;
        private readonly IDataFileObjectManager _dataFileObjectManager;
        private readonly IVerificationCodeService _verificationCodeService;

        public TokenAuthController(
            LogInManager logInManager,
            ITenantCache tenantCache,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            TokenAuthConfiguration configuration,
            IExternalAuthConfiguration externalAuthConfiguration,
            IExternalAuthManager externalAuthManager,
            UserRegistrationManager userRegistrationManager,
            ICacheManager cacheManager,
            IImpersonationManager impersonationManager,
            IUserLinkManager userLinkManager,
            IOptions<IdentityOptions> identityOptions,
               IVerificationCodeService verificationCodeService,
            UserManager userManager, IDataFileObjectManager dataFileObjectManager
            )
        {
            _logInManager = logInManager;
            _tenantCache = tenantCache;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _configuration = configuration;
            _externalAuthConfiguration = externalAuthConfiguration;
            _externalAuthManager = externalAuthManager;
            _userRegistrationManager = userRegistrationManager;
            _cacheManager = cacheManager;
            _impersonationManager = impersonationManager;
            _userLinkManager = userLinkManager;
            _identityOptions = identityOptions.Value;
            _userManager = userManager;
            _dataFileObjectManager = dataFileObjectManager;
            _verificationCodeService = verificationCodeService;
        }

        [HttpPost]
        public async Task<AuthenticateResultModel> Authenticate([FromBody] AuthenticateModel model)
        {
            // 校验验证码
            //      await this.ChcekVerificationCode(model);

            var loginResult = await GetLoginResultAsync(
                model.UserNameOrEmailAddress,
                model.Password,
                GetTenancyNameOrNull()
            );

            var returnUrl = model.ReturnUrl;

            // 需要进行账号绑定激活
            if (loginResult.Result == AbpLoginResultType.UserIsNotActive)
            {
                return new AuthenticateResultModel
                {
                    WaitingForActivation = true,
                    UserId = loginResult.User.Id
                };
            }

            // 密码需要重置
            if (loginResult.User.NeedToChangeThePassword)
            {
                loginResult.User.SetNewPasswordResetCode();
                return new AuthenticateResultModel
                {
                    ShouldResetPassword = true,
                    PasswordResetCode = loginResult.User.PasswordResetCode,
                    UserId = loginResult.User.Id,
                    ReturnUrl = returnUrl
                };
            }

            var accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));

            if (!string.IsNullOrEmpty(model.AuthProvider))
            {
                await _userManager.AddLoginAsync(loginResult.User, new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider));
            }

            return new AuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                UserId = loginResult.User.Id,
                ReturnUrl = returnUrl
            };
        }

        [HttpGet]
        public List<ExternalLoginProviderInfoModel> GetExternalAuthenticationProviders()
        {
            return ObjectMapper.Map<List<ExternalLoginProviderInfoModel>>(_externalAuthConfiguration.Providers);
        }

        /// <summary>
        /// 扩展登录授权
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ExternalAuthenticateResultModel> ExternalAuthenticate([FromBody] ExternalAuthenticateModel model)
        {
            var externalUser = await GetExternalUserInfo(model);

            // 判断是否有这个Key
            var tenancyName = GetTenancyNameOrNull();
            var loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    {
                        var accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));
                        return new ExternalAuthenticateResultModel
                        {
                            AccessToken = accessToken,
                            EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                            ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                            UserId = loginResult.User.Id

                        };
                    }
                case AbpLoginResultType.UnknownExternalLogin:
                case AbpLoginResultType.UserIsNotActive:
                    if (loginResult.User == null)
                    {
                        // 该第三方key未绑定用户表信息
                        return new ExternalAuthenticateResultModel
                        {
                            ProviderKey = model.ProviderKey,
                            WaitingForActivation = false,
                            UserId = 0
                        };
                    }
                    else
                    {
                        // 找到该用户，但是处于锁定状态
                        return new ExternalAuthenticateResultModel
                        {
                            ProviderKey = model.ProviderKey,
                            WaitingForActivation = true,
                            UserId = (await _userManager.GetUserByEmail(externalUser.EmailAddress)).Id
                        };
                    }
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                         loginResult.Result,
                         model.ProviderKey,
                         GetTenancyNameOrNull()
                     );
            }


            #region 旧版本
            //switch (loginResult.Result)
            //{
            //    case AbpLoginResultType.Success:
            //        {
            //            string accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));
            //            return new ExternalAuthenticateResultModel
            //            {
            //                AccessToken = accessToken,
            //                EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
            //                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
            //                UserId = loginResult.User.Id

            //            };
            //        }
            //    case AbpLoginResultType.UnknownExternalLogin:
            //        {
            //            //注册新用户到数据库中
            //            User newUser = await RegisterExternalUserAsync(externalUser);
            //            if (!newUser.IsActive)
            //            {
            //                return new ExternalAuthenticateResultModel
            //                {
            //                    WaitingForActivation = true,
            //                    UserId = newUser.Id,
            //                    ProviderKey = externalUser.ProviderKey,

            //                };
            //            }

            //            // Try to login again with newly registered user!
            //            loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());
            //            if (loginResult.Result != AbpLoginResultType.Success)
            //            {
            //                throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
            //                    loginResult.Result,
            //                    model.ProviderKey,
            //                    GetTenancyNameOrNull()
            //                );
            //            }

            //            return new ExternalAuthenticateResultModel
            //            {
            //                AccessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User)),
            //                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
            //            };
            //        }
            //    case AbpLoginResultType.UserIsNotActive:
            //        {
            //            // 需要进行账号绑定激活
            //            return new ExternalAuthenticateResultModel
            //            {
            //                WaitingForActivation = true,
            //                UserId = (await _userManager.GetUserByEmail(externalUser.EmailAddress)).Id
            //            };
            //        }
            //    default:
            //        {
            //            throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
            //                loginResult.Result,
            //                model.ProviderKey,
            //                GetTenancyNameOrNull()
            //            );
            //        }
            // }
            #endregion

        }

        [HttpPost]
        public async Task<ActivateAccountResultModel> ActivateAccount([FromBody] ActivateAccountModel model)
        {
            if (model.ActivateType == ActivateType.NewAccount)
            {
                await ActivateWithNewAccount(model);
            }
            else
            {
                await ActivateWithExistAccount(model);
            }

            var loginResult = await GetLoginResultAsync(
                model.EmailAddress,
                model.Password,
                GetTenancyNameOrNull()
            );

            if (loginResult.Result != AbpLoginResultType.Success)
            {
                var ex = new UserFriendlyException("ActivateAccountFailed");
                Logger.Warn(new { loginResult.Result, loginResult.User.Id }.ToJsonString(), ex);
                throw ex;
            }

            var accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));

            return new ActivateAccountResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                UserId = loginResult.User.Id,
            };
        }

        [HttpPost]
        public async Task<ImpersonatedAuthenticateResultModel> ImpersonatedAuthenticate(string impersonationToken)
        {
            var result = await _impersonationManager.GetImpersonatedUserAndIdentity(impersonationToken);
            var accessToken = CreateAccessToken(await CreateJwtClaims(result.Identity, result.User));

            return new ImpersonatedAuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
            };
        }

        [HttpPost]
        public async Task<SwitchedAccountAuthenticateResultModel> LinkedAccountAuthenticate(string switchAccountToken)
        {
            var result = await _userLinkManager.GetSwitchedUserAndIdentity(switchAccountToken);
            var accessToken = CreateAccessToken(await CreateJwtClaims(result.Identity, result.User));

            return new SwitchedAccountAuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
            };
        }




        #region 私有方法

        private async Task ActivateWithExistAccount(ActivateAccountModel model)
        {

            var targetUser = await _userManager.GetUserByEmail(model.EmailAddress);
            var userToDelete = await _userManager.FindByIdAsync(model.UserId);
            var loginInfo = (await _userManager.GetLoginsAsync(userToDelete)).First();
            await _userManager.RemoveLoginAsync(userToDelete, loginInfo.LoginProvider,
                loginInfo.ProviderKey);
            await _userManager.AddLoginAsync(targetUser, loginInfo);
            var result = await _userManager.DeleteAsync(userToDelete);
            result.CheckErrors(LocalizationManager);
        }

        /// <summary>
        /// 激活到账户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task ActivateWithNewAccount(ActivateAccountModel model)
        {
            // await this.ChcekVerificationCode(model.EmailAddress, model.VerificationCode);
            var targetUser = await _userManager.GetUserByIdAsync(long.Parse(model.UserId));
            targetUser.IsActive = true;
            var changePasswordResult = await _userManager.ChangePasswordAsync(targetUser, model.Password);
            changePasswordResult.CheckErrors(LocalizationManager);
            targetUser.EmailAddress = targetUser.UserName = model.EmailAddress;
            var result = await _userManager.UpdateAsync(targetUser);
            result.CheckErrors(LocalizationManager);
        }

        private async Task<User> RegisterExternalUserAsync(ExternalAuthUserInfo externalUser)
        {
            var user = await _userRegistrationManager.RegisterAsync(
                externalUser.Name,
                externalUser.EmailAddress,
                externalUser.EmailAddress,
                UserManagerment.Users.User.CreateRandomPassword(),
                true
            );
            if (Uri.TryCreate(externalUser.AvatarUrl, UriKind.Absolute, out var uri))
            {
                using (var httpClient = new HttpClient())
                {
                    var avatarBytes = await httpClient.GetByteArrayAsync(uri);
                    var storedFile = new DataFileObject(AbpSession.TenantId, avatarBytes);
                    await _dataFileObjectManager.SaveAsync(storedFile);
                    user.ProfilePictureId = storedFile.Id;
                }
            }
            user.Logins = new List<UserLogin>
            {
                new UserLogin
                {
                    LoginProvider = externalUser.Provider,
                    ProviderKey = externalUser.ProviderKey,
                    TenantId = user.TenantId
                }
            };

            await CurrentUnitOfWork.SaveChangesAsync();

            return user;
        }
        /// <summary>
        /// 获取扩展登录的用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<ExternalAuthUserInfo> GetExternalUserInfo(ExternalAuthenticateModel model)
        {
            var userInfo = await _externalAuthManager.GetUserInfo(model.AuthProvider, model.ProviderAccessCode);
            if (userInfo.ProviderKey != model.ProviderKey)
            {
                throw new UserFriendlyException(L("CouldNotValidateExternalUser"));
            }

            return userInfo;
        }

        /// <summary>
        /// 获取租户名称
        /// </summary>
        /// <returns></returns>
        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        /// <summary>
        /// 获取登陆结果
        /// </summary>
        /// <param name="usernameOrEmailAddress"></param>
        /// <param name="password"></param>
        /// <param name="tenancyName"></param>
        /// <returns></returns>
        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;

                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        /// <summary>
        /// 创建jwt token
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? _configuration.Expiration),
                signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private async Task<IEnumerable<Claim>> CreateJwtClaims(ClaimsIdentity identity, User user, TimeSpan? expiration = null)
        {
            var tokenValidityKey = Guid.NewGuid().ToString();
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == _identityOptions.ClaimsIdentity.UserIdClaimType);

            if (_identityOptions.ClaimsIdentity.UserIdClaimType != JwtRegisteredClaimNames.Sub)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value));
            }

            var userIdentifier = new UserIdentifier(AbpSession.TenantId, Convert.ToInt64(nameIdClaim.Value));

            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(AbpProConsts.TokenValidityKey, tokenValidityKey),
                new Claim(AbpProConsts.UserIdentifier, userIdentifier.ToUserIdentifierString())
            });

            // 创建token验证缓存
            _cacheManager
                .GetCache(AbpProConsts.TokenValidityKey)
                .Set(tokenValidityKey, "");

            // 更新用户token
            await _userManager.AddTokenValidityKeyAsync(user, tokenValidityKey,
                DateTime.UtcNow.Add(expiration ?? _configuration.Expiration));

            return claims;
        }

        /// <summary>
        /// 获取编码后的jwt token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        private string GetEncrpyedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
        }

        /// <summary>
        /// 检查验证码
        /// </summary>
        /// <returns></returns>
        private async Task ChcekVerificationCode(AuthenticateModel model)
        {
            // 检查验证码
            //await CaptchaHelper.CheckVerificationCode(
            //    this._cacheManager,
            //    this.SettingManager,
            //    this.AbpSession.TenantId.HasValue ? CaptchaType.TenantUserLogin : CaptchaType.HostUserLogin,
            //    email,
            //    verificationCode,
            //    this.AbpSession.TenantId);

            if (model.VerificationCode.IsNullOrWhiteSpace())
            {
                throw new UserFriendlyException("请输入有效验证码!");
            }

            // 分租户获取验证码缓存
            var cacheKey = CacheConsts.HostApi_TokenAuth_Authenticate;
            if (AbpSession.TenantId.HasValue)
            {
                cacheKey = $"{AbpSession.TenantId}-{CacheConsts.HostApi_TokenAuth_Authenticate}";
            }
            var cache = _cacheManager.GetCache(cacheKey).AsTyped<string, string>();
            var cacheValue = await cache?.GetOrDefaultAsync(model.UserNameOrEmailAddress);
            if (cacheValue.IsNullOrWhiteSpace())
            {
                // 从缓存中移除验证码
                await cache.RemoveAsync(model.UserNameOrEmailAddress);
                throw new UserFriendlyException("验证码已过期,请刷新验证码图片后重试");
            }

            // 对比验证码
            if (model.VerificationCode.ToLower() != cacheValue)
            {
                // 从缓存中移除验证码
                await cache.RemoveAsync(model.UserNameOrEmailAddress);
                throw new UserFriendlyException("验证码错误!");
            }

            // 从缓存中移除验证码
            await cache.RemoveAsync(model.UserNameOrEmailAddress);
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="name">键值</param>
        /// <param name="t">验证码类型</param>
        /// <param name="l">长度</param>
        /// <param name="tid">租户id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<FileContentResult> GenerateVerification(string name, ValidateCodeType? t, int? l, long? tid)
        {
            if (name.IsNullOrWhiteSpace())
            {
                return null;
            }

            if (!t.HasValue)
            {
                t = ValidateCodeType.Number;
            }
            if (!l.HasValue)
            {
                l = 6;
            }


            var imgStream = _verificationCodeService.Create(out var code, t.Value, l.Value);

            // 分租户获取缓存
            var cacheKey = CacheConsts.HostApi_TokenAuth_Authenticate;
            if (tid.HasValue)
            {
                cacheKey = $"{tid}-{CacheConsts.HostApi_TokenAuth_Authenticate}";
            }
            var cache = _cacheManager.GetCache(cacheKey);
            cache.DefaultAbsoluteExpireTime = TimeSpan.FromMinutes(3);

            // 存值,过期时间3分钟
            await cache.SetAsync(name, code.ToLower());

            //
            Response.Body.Dispose();
            return File(imgStream.ToArray(), MimeTypeNames.ImagePng);
        }

        #endregion 私有方法
    }
}

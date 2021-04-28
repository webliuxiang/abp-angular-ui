using System.Collections.Generic;
using System.Security.Claims;
using Abp.Dependency;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.Dto;
using Microsoft.AspNetCore.Identity;

namespace LTMCompanyName.YoyoCmsTemplate.Authentication.External
{
    public interface IExternalLoginInfoManager : ITransientDependency
    {
        string GetUserNameFromClaims(List<Claim> claims);

        string GetUserNameFromExternalAuthUserInfo(ExternalAuthUserInfo userInfo);

        (string name, string surname) GetNameAndSurnameFromClaims(List<Claim> claims, IdentityOptions identityOptions);
    }
}
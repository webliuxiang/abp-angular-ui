using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.Dto;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace LTMCompanyName.YoyoCmsTemplate.Authentication.External.ExternalAuth.API
{
    public class OpenIdConnectAuthProviderApi : ExternalAuthProviderApiBase
    {
        public const string Name = "OpenIdConnect";

        public override async Task<ExternalAuthUserInfo> GetUserInfo(string token)
        {
            string text = base.ProviderInfo.AdditionalParams["Authority"];
            if (string.IsNullOrEmpty(text))
            {
                throw new ApplicationException("Authentication:OpenId:Issuer configuration is required.");
            }
            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(text + "/.well-known/openid-configuration", new OpenIdConnectConfigurationRetriever(), new HttpDocumentRetriever());
            var jwtToken = await ValidateToken(token, text, configurationManager, default(CancellationToken));

            if (jwtToken == null)
            {
                throw new NullReferenceException("ValidateToken Token is Null!");
            }

            var name = jwtToken.Claims.First((Claim c) => c.Type == "name").Value;
            var unique_name = jwtToken.Claims.First((Claim c) => c.Type == "unique_name").Value;
            var array = name.Split(' ', StringSplitOptions.None);

            return new ExternalAuthUserInfo
            {
                Provider = "OpenIdConnect",
                ProviderKey = jwtToken.Subject,
                Name = array[0],
                //Surname = array[1],
                EmailAddress = unique_name
            };
        }

        private async Task<JwtSecurityToken> ValidateToken(string token, string issuer, IConfigurationManager<OpenIdConnectConfiguration> configurationManager, CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException("token");
            }
            if (string.IsNullOrEmpty(issuer))
            {
                throw new ArgumentNullException("issuer");
            }
            var signingKeys = (await configurationManager.GetConfigurationAsync(ct)).SigningKeys;

            var tokenValidationParameters = new TokenValidationParameters();
            tokenValidationParameters.ValidateIssuer = true;
            tokenValidationParameters.ValidIssuer = issuer;
            tokenValidationParameters.ValidateIssuerSigningKey = true;
            tokenValidationParameters.IssuerSigningKeys = signingKeys;
            tokenValidationParameters.ValidateLifetime = true;
            tokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(5.0);
            tokenValidationParameters.ValidateAudience = false;

            var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out SecurityToken jwtToken);
            if (base.ProviderInfo.ClientId != claimsPrincipal.Claims.First((Claim c) => c.Type == "aud").Value)
            {
                throw new ApplicationException("ClientId couldn't verified.");
            }
            return jwtToken as JwtSecurityToken;
        }
    }
}
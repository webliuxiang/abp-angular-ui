using System;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Startup
{
    public static class AuthConfigurer
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            var authenticationBuilder = services.AddAuthentication();

            if (bool.Parse(configuration["Authentication:JwtBearer:IsEnabled"]))
            {
                authenticationBuilder.AddJwtBearer(options =>
                {
                    options.Audience = configuration["Authentication:JwtBearer:Audience"];

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // The signing key must match!
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"])),

                        // Validate the JWT Issuer (iss) claim
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Authentication:JwtBearer:Issuer"],

                        // Validate the JWT Audience (aud) claim
                        ValidateAudience = true,
                        ValidAudience = configuration["Authentication:JwtBearer:Audience"],

                        // Validate the token expiry
                        ValidateLifetime = true,

                        // If you want to allow a certain amount of clock drift, set that here
                        ClockSkew = TimeSpan.Zero
                    };
                });
            }

            //qq login config
            if (bool.Parse(configuration["Authentication:QQ:IsEnabled"]))
            {
                authenticationBuilder.AddQQ(options =>
                {
                    options.ClientId = configuration["Authentication:QQ:ClientId"];
                    options.ClientSecret = configuration["Authentication:QQ:ClientSecret"];
                });
            }

            if (bool.Parse(configuration["Authentication:Microsoft:IsEnabled"]))
            {
                authenticationBuilder.AddMicrosoftAccount(options =>
                {
                    options.ClientId = configuration["Authentication:Microsoft:ConsumerKey"];
                    options.ClientSecret = configuration["Authentication:Microsoft:ConsumerSecret"];
                });
            }

            if (bool.Parse(configuration["Authentication:Google:IsEnabled"]))
            {
                authenticationBuilder.AddGoogle(options =>
                {
                    options.ClientId = configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = configuration["Authentication:Google:ClientSecret"];

                    options.UserInformationEndpoint = configuration["Authentication:Google:UserInfoEndpoint"];
                    options.ClaimActions.Clear();

                    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                    options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                });
            }

            if (bool.Parse(configuration["Authentication:Twitter:IsEnabled"]))
            {
                //authenticationBuilder.AddTwitter(options =>
                //{
                //    options.ConsumerKey = configuration["Authentication:Twitter:ConsumerKey"];
                //    options.ConsumerSecret = configuration["Authentication:Twitter:ConsumerSecret"];
                //    options.RetrieveUserDetails = true;
                //});
            }

            if (bool.Parse(configuration["Authentication:Facebook:IsEnabled"]))
            {
                authenticationBuilder.AddFacebook(options =>
                {
                    options.AppId = configuration["Authentication:Facebook:AppId"];
                    options.AppSecret = configuration["Authentication:Facebook:AppSecret"];

                    options.Scope.Add("email");
                    options.Scope.Add("public_profile");
                });
            }
        }
    }
}

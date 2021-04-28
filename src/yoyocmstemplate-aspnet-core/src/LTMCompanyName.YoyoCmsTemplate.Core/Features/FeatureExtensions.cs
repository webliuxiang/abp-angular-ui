using Abp.Application.Features;
using Abp.Localization;
using Microsoft.AspNetCore.Builder;

namespace LTMCompanyName.YoyoCmsTemplate.Features
{
    public static class FeatureExtensions
    {
        public static string GetValueText(this Feature feature, string value, ILocalizationContext localizationContext)
        {
            var featureMetadata = feature[FeatureMetadata.CustomFeatureKey] as FeatureMetadata;
            if (featureMetadata?.ValueTextNormalizer == null)
            {
                return value;
            }

            return featureMetadata.ValueTextNormalizer(value).Localize(localizationContext);
        }
        public static void UseDefaultTenantEnforcer(this IApplicationBuilder app)
        {
            app.Use((context, func) =>
            {
                if (!context.Request.Cookies.ContainsKey("Abp.TenantId"))
                {
                    context.Response.Cookies.Append("Abp.TenantId", "1");
                }

                return func.Invoke();
            });
        }
    }
}

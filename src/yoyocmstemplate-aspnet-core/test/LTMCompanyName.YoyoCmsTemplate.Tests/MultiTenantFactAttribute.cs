using Xunit;

namespace LTMCompanyName.YoyoCmsTemplate.Tests
{
    public sealed class MultiTenantFactAttribute : FactAttribute
    {
        public MultiTenantFactAttribute()
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            //if (!AppConsts.MultiTenancyEnabled)
            //{
            //    Skip = "MultiTenancy is disabled.";
            //}
        }
    }
}


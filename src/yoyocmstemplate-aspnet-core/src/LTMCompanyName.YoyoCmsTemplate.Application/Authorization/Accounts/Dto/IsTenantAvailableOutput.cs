namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Accounts.Dto
{
    public class IsTenantAvailableOutput
    {
        public IsTenantAvailableOutput()
        {
        }

        public IsTenantAvailableOutput(TenantAvailabilityState state, int? tenantId = null)
        {
            State = state;
            TenantId = tenantId;
        }

        public TenantAvailabilityState State { get; set; }

        public int? TenantId { get; set; }
    }
}
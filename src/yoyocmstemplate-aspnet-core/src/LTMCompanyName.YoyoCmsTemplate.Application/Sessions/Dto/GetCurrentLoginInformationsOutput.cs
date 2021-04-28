using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Sessions.Dto
{
    public class GetCurrentLoginInformationsOutput
    {
        public UserLoginInfoDto User { get; set; }

        public TenantLoginInfoDto Tenant { get; set; }

        public ApplicationInfoDto Application { get; set; }
    }
}

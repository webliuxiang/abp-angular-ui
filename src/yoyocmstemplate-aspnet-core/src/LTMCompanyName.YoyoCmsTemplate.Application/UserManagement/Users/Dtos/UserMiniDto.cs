using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos
{
    public class UserMiniDto : EntityDto<long>
    {
        public string UserName { get; set; }
    }
}

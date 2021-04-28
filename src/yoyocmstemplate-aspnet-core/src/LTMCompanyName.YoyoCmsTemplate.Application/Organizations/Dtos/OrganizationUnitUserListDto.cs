using System;
using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos
{

    public class OrganizationUnitUserListDto : EntityDto<long>
    {
        public string UserName { get; set; }

        public DateTime AddedTime { get; set; }
    }
}
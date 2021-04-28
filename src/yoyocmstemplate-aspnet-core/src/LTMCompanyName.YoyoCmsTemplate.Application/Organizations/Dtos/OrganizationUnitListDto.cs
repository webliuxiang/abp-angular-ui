using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos
{
    /// <summary>
    /// 组织机构列表Dto
    /// </summary>
    public class OrganizationUnitListDto : AuditedEntityDto<long>
    {
        public long? ParentId { get; set; }

        public string Code { get; set; }

        public string DisplayName { get; set; }

        public int MemberCount { get; set; }

        public int RoleCount { get; set; }
    }
}
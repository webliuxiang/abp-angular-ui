using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Roles.Dtos
{
    [AutoMap(typeof(Role))]
    public class RoleEditDto
    {
        public int? Id { get; set; }

        [Required] public string DisplayName { get; set; }

        public bool IsDefault { get; set; }
    }
}
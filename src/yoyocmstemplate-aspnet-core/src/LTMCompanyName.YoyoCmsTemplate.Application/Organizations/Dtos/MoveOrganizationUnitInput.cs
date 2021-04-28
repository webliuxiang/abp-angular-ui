using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos
{
    public class MoveOrganizationUnitInput
    {
        [Range(1, long.MaxValue)] public long Id { get; set; }

        public long? NewParentId { get; set; }
    }
}
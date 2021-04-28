using System.Collections.Generic;

namespace LTMCompanyName.YoyoCmsTemplate.Authorization.Permissions.Dtos
{
    public class TreePermissionDto
    {
        public string ParentName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public List<TreePermissionDto> Children { get; set; }

        public bool Checked { get; set; }
    }
}
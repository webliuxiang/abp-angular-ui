using System;
using Abp;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos.Enqueue
{
    public class ImportUsersFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}

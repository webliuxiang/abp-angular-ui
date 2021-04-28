using System;

namespace LTMCompanyName.YoyoCmsTemplate.SystemBaseManage
{
    public class MoveSysFilesInput
    {
        public Guid Id { get; set; }

        public Guid? NewParentId { get; set; }
    }
}

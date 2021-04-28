using System;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos
{
    public class RecentTenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
    }
}

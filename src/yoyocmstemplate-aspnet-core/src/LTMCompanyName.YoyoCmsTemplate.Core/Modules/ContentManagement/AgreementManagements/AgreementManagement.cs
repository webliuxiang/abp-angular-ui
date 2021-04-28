using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.ContentManagement.AgreementManagements
{

    /// <summary>
    /// 协议管理
    /// </summary>
    public class AgreementManagement : FullAuditedEntity<long>, IMayHaveTenant
    {
        //todo:如用户协议等内容


        public int? TenantId { get; set; }


        public string Version { get; set; }


    }
}

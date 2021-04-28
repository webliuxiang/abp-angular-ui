using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.ContentManagement.Feedbacks
{
    /// <summary>
    /// 反馈信息
    /// </summary>
    public class Feedback : CreationAuditedEntity<long>, IMayHaveTenant, ISoftDelete
    {
        //todo:可能不需要 功能

        public int? TenantId { get; set; }

        public bool IsDeleted { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
        public string CellPhone { get; set; }
        public string ContactAddress { get; set; }

        /// <summary>
        /// 反馈内容
        /// </summary>
        public string FeedbackContent { get; set; }

        /// <summary>
        /// 客户端Ip
        /// </summary>
        public string ClientIp { get; set; }


        /// <summary>
        /// 浏览器
        /// </summary>
        public string BrowserInfo { get; set; }





    }
}

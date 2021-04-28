using Abp.Domain.Entities;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.ContentManagement.Members
{
    /// <summary>
    /// 会员信息，扩展
    /// </summary>
    public class Member : Entity<long>
    {
        /// <summary>
        /// 所在地
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 关于我
        /// </summary>
        public string AboutMe { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        public string Wechat { get; set; }

        /// <summary>
        /// QQ号
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// GitHub
        /// </summary>
        public string GitHub { get; set; }

        /// <summary>
        /// 码云
        /// </summary>
        public string Gitee { get; set; }

        /// <summary>
        /// 关联的用户Id
        /// </summary>
        public long UserId { get; set; }

    }
}

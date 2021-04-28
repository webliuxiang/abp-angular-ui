using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LTMCompanyName.YoyoCmsTemplate.WechatManagement;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs
{
    /// <summary>
    /// 微信基本信息配置
    /// </summary>
    [Table(AppConsts.TablePrefix + "WechatAppConfigs")]

    public class WechatAppConfig : AuditedEntity, IMayHaveTenant
    {

        #region 微信公众号核心信息

        /// <summary>
        /// 微信AppID
        /// </summary>
        [StringLength(250)]
        public string AppId { get; set; }

        /// <summary>
        /// 微信AppSecret
        /// </summary>
        [StringLength(250)]
        public string AppSecret { get; set; }

        /// <summary>
        /// 微信Token
        /// </summary>
        [StringLength(250)]
        public string Token { get; set; }


        /// <summary>
        /// 微信EncodingAESKey
        /// </summary>
        [StringLength(500)]
        public string EncodingAESKey { get; set; }


        #endregion


        #region 微信公众号附加信息

        /// <summary>
        /// 微信号名
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 微信原始ID
        /// </summary>
        [StringLength(250)]
        public string AppOrgId { get; set; }

        /// <summary>
        /// 微信类型（枚举 订阅号、认证订阅号、服务号、认证服务号）
        /// </summary>
        public WechatAppTypeEnum AppType { get; set; }

        /// <summary>
        /// 微信二维码图片URL
        /// </summary>
        [StringLength(250)]
        public string QRCodeUrl { get; set; }

        #endregion
        /// <summary>
        /// 租户ID
        /// </summary>
        public int? TenantId { get; set; }
    }
}

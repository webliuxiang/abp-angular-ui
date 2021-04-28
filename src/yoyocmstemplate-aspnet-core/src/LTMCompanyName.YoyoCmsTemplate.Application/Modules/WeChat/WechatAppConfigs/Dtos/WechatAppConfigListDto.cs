using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.WechatManagement;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs.Dtos
{
    public class WechatAppConfigListDto : AuditedEntityDto
    {
        /// <summary>
        /// AppId
        /// </summary>
        [Required(ErrorMessage = "AppId不能为空")]
        public string AppId { get; set; }



        /// <summary>
        /// 公众号名称
        /// </summary>
        [Required(ErrorMessage = "公众号名称不能为空")]
        public string Name { get; set; }



        /// <summary>
        /// 公众号类型
        /// </summary>
        public WechatAppTypeEnum AppType { get; set; }

        /// <summary>
        /// 公众号类型中文名称
        /// </summary>
        public string AppTypeStr { get; set; }

        /// <summary>
        /// QRCodeUrl
        /// </summary>
        public string QRCodeUrl { get; set; }

        /// <summary>
        /// 已注册到应用冲
        /// </summary>
        public bool Registered { get; set; }
    }
}

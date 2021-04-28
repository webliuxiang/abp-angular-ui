using System.ComponentModel.DataAnnotations;
using LTMCompanyName.YoyoCmsTemplate.WechatManagement;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatAppConfigs.Dtos
{
    public class WechatAppConfigEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public int? Id { get; set; }



        /// <summary>
        /// AppId
        /// </summary>
        [Required(ErrorMessage = "AppId不能为空")]
        public string AppId { get; set; }



        /// <summary>
        /// AppSecret
        /// </summary>
        [Required(ErrorMessage = "AppSecret不能为空")]
        public string AppSecret { get; set; }



        /// <summary>
        /// Token
        /// </summary>
        [Required(ErrorMessage = "Token不能为空")]
        public string Token { get; set; }



        /// <summary>
        /// EncodingAESKey
        /// </summary>
        public string EncodingAESKey { get; set; }



        /// <summary>
        /// 公众号名称
        /// </summary>
        [Required(ErrorMessage = "公众号名称不能为空")]
        public string Name { get; set; }



        /// <summary>
        /// 公众号原始ID
        /// </summary>
        public string AppOrgId { get; set; }



        /// <summary>
        /// 公众号类型
        /// </summary>
        [Required(ErrorMessage = "公众号类型不能为空")]
        public WechatAppTypeEnum AppType { get; set; }



        /// <summary>
        /// QRCodeUrl
        /// </summary>
        public string QRCodeUrl { get; set; }




    }
}

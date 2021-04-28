using System.ComponentModel.DataAnnotations;
using L._52ABP.Application.Dtos;
using Senparc.Weixin.MP;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatMedias.Dtos
{
    public class GetOtherMaterialsInput : PagedInputDto
    {
        [Required]
        public string AppId { get; set; }

        [Required]
        public UploadMediaFileType MaterialType { get; set; }
    }
}

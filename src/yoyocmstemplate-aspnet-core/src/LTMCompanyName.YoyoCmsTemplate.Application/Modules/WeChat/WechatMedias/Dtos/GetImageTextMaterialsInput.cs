using System.ComponentModel.DataAnnotations;
using L._52ABP.Application.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatMedias.Dtos
{
    public class GetImageTextMaterialsInput : PagedInputDto
    {
        [Required]
        public string AppId { get; set; }
    }
}

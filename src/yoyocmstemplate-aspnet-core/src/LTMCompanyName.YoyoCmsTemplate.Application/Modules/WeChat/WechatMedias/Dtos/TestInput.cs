using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WeChat.WechatMedias.Dtos
{
    public class TestInput
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public string Data { get; set; }
    }

}

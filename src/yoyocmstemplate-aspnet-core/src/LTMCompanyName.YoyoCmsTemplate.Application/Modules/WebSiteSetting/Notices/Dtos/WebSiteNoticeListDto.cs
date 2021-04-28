using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;


namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Notices.Dtos
{
    /// <summary>
    /// 网站公告的编辑DTO
    /// <see cref="WebSiteNotice"/>
    /// </summary>
    public class WebSiteNoticeListDto : AuditedEntityDto<long>
    {


        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        public string Title { get; set; }



        /// <summary>
        /// 内容
        /// </summary>
        [MaxLength(3000)]
        [Required(ErrorMessage = "内容不能为空")]
        public string Content { get; set; }



        /// <summary>
        /// ViewCount
        /// </summary>
        public int ViewCount { get; set; }




        //// custom codes



        //// custom codes end
    }
}
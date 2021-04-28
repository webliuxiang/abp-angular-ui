using System.ComponentModel.DataAnnotations;


namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Notices.Dtos
{
    /// <summary>
    /// 网站公告的列表DTO
    /// <see cref="WebSiteNotice"/>
    /// </summary>
    public class WebSiteNoticeEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }



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
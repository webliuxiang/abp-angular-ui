using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Notices
{

    /// <summary>
    /// 网站公告
    /// </summary>
    [Table(AppConsts.TablePrefix + "WebSiteNotices")]

    public class WebSiteNotice : AuditedEntity<long>
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "公告标题不能为空！")]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required(ErrorMessage = "公告内容不能为空！")]
        [MaxLength(3000)]
        public string Content { get; set; }



        /// <summary>
        /// 浏览人数
        /// </summary>
        public int ViewCount { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls
{

    /// <summary>
    /// 友情链接
    /// </summary>
    [Table(AppConsts.TablePrefix + "Blogrolls")]

    public class Blogroll : AuditedEntity
    {

        /// <summary>
        /// 友情链接分类id
        /// </summary>
        public int BlogrollTypeId { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        [Required(ErrorMessage = "站点名不能为空！")]
        public string Name { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        [Required(ErrorMessage = "站点的URL不能为空！")]
        public string Url { get; set; }

        /// <summary>
        /// 是否检测白名单
        /// </summary>
        public bool Except { get; set; }

        /// <summary>
        /// 是否是推荐站点
        /// </summary>
        public bool Recommend { get; set; }

        /// <summary>
        /// 友链权重
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// logo
        /// </summary>
        public string Logo { get; set; }


        /// <summary>
        /// 图标名称
        /// </summary>
        public string IconName { get; set; }

    }
}

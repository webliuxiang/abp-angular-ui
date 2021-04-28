using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls
{


    /// <summary>
    /// 友情链接分类
    /// </summary>
    [Table(AppConsts.TablePrefix + "BlogrollTypes")]

    public class BlogrollType : AuditedEntity
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        [MaxLength(100)]

        public string Name { get; set; }



    }
}

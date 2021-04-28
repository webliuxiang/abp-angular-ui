using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs
{
    /// <summary>
    /// 贴子标签
    /// </summary>
    [Table(AppConsts.TablePrefix + "BlogTag")]

    public class BlogTag : CreationAuditedEntity
    {
        public virtual Guid BlogId { get; protected set; }

        public virtual Guid TagId { get; protected set; }

        protected BlogTag()
        {

        }

        public BlogTag(Guid blogId, Guid tagId)
        {
            BlogId = blogId;
            TagId = tagId;
        }


    }
}

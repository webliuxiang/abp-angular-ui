using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts
{
    /// <summary>
    /// 贴子标签
    /// </summary>
    [Table(AppConsts.TablePrefix + "PostTags")]

    public class PostTag : CreationAuditedEntity
    {
        public virtual Guid PostId { get; protected set; }

        public virtual Guid TagId { get; protected set; }

        protected PostTag()
        {

        }

        public PostTag(Guid postId, Guid tagId)
        {
            PostId = postId;
            TagId = tagId;
        }


    }
}

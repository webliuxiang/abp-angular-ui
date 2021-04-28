using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Comments
{

    /// <summary>
    /// 评论
    /// </summary>
    [Table(AppConsts.TablePrefix + "Comments")]

    public class Comment : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 评论的文章
        /// </summary>
        public virtual Guid PostId { get; protected set; } 

        /// <summary>
        /// 回复的评论
        /// </summary>
        public virtual Guid? RepliedCommentId { get; protected set; }

        public virtual string Text { get; protected set; }

        protected Comment()
        {

        }

        public Comment(Guid id, Guid postId, Guid? repliedCommentId, [NotNull] string text)
        {
            Id = id;
            PostId = postId;
            RepliedCommentId = repliedCommentId;
            Text = Check.NotNullOrWhiteSpace(text, nameof(text));
        }

        public void SetText(string text)
        {
            Text = Check.NotNullOrWhiteSpace(text, nameof(text));
        }
    }
}

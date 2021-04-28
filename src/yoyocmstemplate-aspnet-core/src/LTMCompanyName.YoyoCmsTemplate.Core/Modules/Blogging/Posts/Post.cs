using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.Enums;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts
{
    /// <summary>
    /// 博客贴子
    /// </summary>
    [Table(AppConsts.TablePrefix + "Posts")]

    public class Post : FullAuditedEntity<Guid>
    {
        public virtual Guid BlogId { get; protected set; }

        [NotNull]
        public virtual string Url { get; protected set; }

        [NotNull]
        public virtual string CoverImage { get; set; }

        [NotNull]
        public virtual string Title { get; protected set; }

        [CanBeNull]
        public virtual string Content { get; set; }

        /// <summary>
        /// 历史内容
        /// </summary>
        public virtual string HistoryContent { get; set; }

        public virtual int ReadCount { get; protected set; }

        public virtual PostType PostType { get; set; }

        public virtual ICollection<PostTag> Tags { get; set; }

        protected Post()
        {
        }

        public Post(Guid id, Guid blogId, [NotNull] string title, [NotNull] string coverImage, [NotNull] string url, PostType postType)
        {
            Id = id;
            BlogId = blogId;

            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Url = Check.NotNullOrWhiteSpace(url, nameof(url));
            CoverImage = Check.NotNullOrWhiteSpace(coverImage, nameof(coverImage));
            PostType = postType;
            Tags = new Collection<PostTag>();
        }
        /// <summary>
        /// 阅读数量加1
        /// </summary>
        /// <returns></returns>

        public virtual Post IncreaseReadCount()
        {
            ReadCount++;
            return this;
        }

        public virtual Post SetTitle([NotNull] string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            return this;
        }

        public virtual Post SetUrl([NotNull] string url)
        {
            Url = Check.NotNullOrWhiteSpace(url, nameof(url));
            return this;
        }

        public virtual void AddTag(Guid tagId)
        {
            Tags.Add(new PostTag(Id, tagId));
        }

        public virtual void RemoveTag(Guid tagId)
        {
            Tags.RemoveAll(t => t.TagId == tagId);
        }
    }
}

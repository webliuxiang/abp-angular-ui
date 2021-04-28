using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs
{
    /// <summary>
    /// 博客模块
    /// </summary>
    [Table(AppConsts.TablePrefix + "Blogs")]
    public class Blog : FullAuditedEntity<Guid>
    {
        [NotNull]
        public virtual string Name { get; protected set; }

        [NotNull]
        public virtual string ShortName { get; protected set; }

        [CanBeNull]
        public virtual string Description { get; set; }


        public User BlogUser { get; set; }

        public virtual ICollection<BlogTag> Tags { get; set; }

        protected Blog()
        {
            Id = SequentialGuidGenerator.Instance.Create();
        }

        public Blog(Guid id, [NotNull] string name, [NotNull] string shortName)
        {
            Id = id;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            ShortName = Check.NotNullOrWhiteSpace(shortName, nameof(shortName));

            Tags = new Collection<BlogTag>();

        }

        public virtual Blog SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            return this;
        }

        public virtual Blog SetShortName(string shortName)
        {
            ShortName = Check.NotNullOrWhiteSpace(shortName, nameof(shortName));
            return this;
        }

        public virtual void AddTag(Guid tagId)
        {
            if(Tags == null)
            {
                Tags = new Collection<BlogTag>();
            }
            Tags.Add(new BlogTag(Id, tagId));
        }
    }
}

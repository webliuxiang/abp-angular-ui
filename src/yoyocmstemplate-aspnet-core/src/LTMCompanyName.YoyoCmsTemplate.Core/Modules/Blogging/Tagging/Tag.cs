using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Tagging
{

    /// <summary>
    /// 标签
    /// </summary>
    [Table(AppConsts.TablePrefix + "Tags")]

    public class Tag : FullAuditedEntity<Guid>
    {

        public virtual string Name { get; protected set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; protected set; }

        /// <summary>
        /// 占用数量
        /// </summary>
        public virtual int UsageCount { get; protected internal set; }

        protected Tag()
        {
            Id = SequentialGuidGenerator.Instance.Create();

        }

        public Tag([NotNull] string name, int usageCount = 0, string description = null) : this()
        {

            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Description = description;
            UsageCount = usageCount;
        }

        public virtual void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        }

        /// <summary>
        /// 添加标签使用数量
        /// </summary>
        /// <param name="number"></param>
        public virtual void IncreaseUsageCount(int number = 1)
        {
            UsageCount += number;
        }

        /// <summary>
        /// 减去标签使用数量
        /// </summary>
        /// <param name="number"></param>
        public virtual void DecreaseUsageCount(int number = 1)
        {
            if (UsageCount <= 0)
            {
                return;
            }

            if (UsageCount - number <= 0)
            {
                UsageCount = 0;
                return;
            }

            UsageCount -= number;
        }

        public virtual void SetDescription(string description)
        {
            Description = description;
        }
    }
}

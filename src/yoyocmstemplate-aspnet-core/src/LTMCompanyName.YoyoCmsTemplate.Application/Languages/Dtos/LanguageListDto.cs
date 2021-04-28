using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Languages.Dtos
{
    public class LanguageListDto : FullAuditedEntityDto
    {
        public virtual int? TenantId { get; set; }

        /// <summary>
        ///     名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        ///     显示名称
        /// </summary>
        public virtual string DisplayName { get; set; }

        /// <summary>
        ///     图标
        /// </summary>
        public virtual string Icon { get; set; }

        /// <summary>
        /// </summary>
        public bool IsDisabled { get; set; }
    }
}
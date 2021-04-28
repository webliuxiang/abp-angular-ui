

using System;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Tagging;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos
{
    /// <summary>
    /// 标签的编辑DTO
    /// <see cref="Tag"/>
    /// </summary>
    public class TagListDto : FullAuditedEntityDto<Guid>
    {

        /// <summary>
        /// 标签名称
        /// </summary>
        public string Name { get; set; }



        /// <summary>
        /// 标签描述
        /// </summary>

        public string Description { get; set; }




        //// custom codes

        /// <summary>
        /// 使用计数器
        /// </summary>
        public int UsageCount { get; set; }



        //// custom codes end
    }
}

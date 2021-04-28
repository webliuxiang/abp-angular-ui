using System;
using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseCategoryInfo.Dtos
{

    /// <summary>
    /// 服务树形列表的公共Dto
    /// </summary>
    public class TreeMemberListDto: EntityDto<long>
    {
        public string Name { get; set; }

        public DateTime AddedTime { get; set; }
    }
}

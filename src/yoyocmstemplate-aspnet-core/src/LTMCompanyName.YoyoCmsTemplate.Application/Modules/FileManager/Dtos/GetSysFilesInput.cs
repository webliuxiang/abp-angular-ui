using System;
using Abp.Runtime.Validation;
using LTMCompanyName.YoyoCmsTemplate.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.SystemBaseManage.Dtos
{
    /// <summary>
    /// 获取文件的传入参数Dto
    /// </summary>
    public class GetSysFilesInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? parentId { get; set; }

        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }

        //// custom codes

        //// custom codes end
    }
}

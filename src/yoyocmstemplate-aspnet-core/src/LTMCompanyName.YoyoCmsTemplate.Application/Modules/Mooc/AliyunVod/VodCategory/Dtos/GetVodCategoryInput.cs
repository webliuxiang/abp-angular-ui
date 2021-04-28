using Abp.Runtime.Validation;
using L._52ABP.Application.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.AliyunVod.VodCategory.Dtos
{
    public class GetVodCategoryInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {


        /// <summary>
        /// 真实分类ID
        /// </summary>
        public  long? CateId { get; set; }
        /// <summary>
        ///     正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CateId";
            }
        }
    }
}

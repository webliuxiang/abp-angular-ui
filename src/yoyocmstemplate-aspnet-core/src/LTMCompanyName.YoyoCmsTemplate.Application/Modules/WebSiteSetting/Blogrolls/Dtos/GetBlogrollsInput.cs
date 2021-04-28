
using Abp.Runtime.Validation;
using LTMCompanyName.YoyoCmsTemplate.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls.Dtos
{
    /// <summary>
    /// 获取友情链接的传入参数Dto
    /// </summary>
    public class GetBlogrollsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {

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

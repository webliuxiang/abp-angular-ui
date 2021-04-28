
using Abp.Runtime.Validation;
using LTMCompanyName.YoyoCmsTemplate.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Notices.Dtos
{
    /// <summary>
    /// 获取网站公告的传入参数Dto
    /// </summary>
    public class GetWebSiteNoticesInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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

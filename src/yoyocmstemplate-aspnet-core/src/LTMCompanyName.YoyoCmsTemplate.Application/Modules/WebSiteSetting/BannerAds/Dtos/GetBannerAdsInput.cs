
using Abp.Runtime.Validation;
using LTMCompanyName.YoyoCmsTemplate.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.BannerAds.Dtos
{
    /// <summary>
    /// 获取轮播图广告的传入参数Dto
    /// </summary>
    public class GetBannerAdsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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

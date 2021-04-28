using Abp.Runtime.Validation;
using LTMCompanyName.YoyoCmsTemplate.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Dtos
{
    /// <summary>
    /// 获取文章的传入参数Dto
    /// </summary>
    public class GetPostsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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

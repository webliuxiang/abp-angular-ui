
using Abp.Runtime.Validation;
using LTMCompanyName.YoyoCmsTemplate.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Comments.Dtos
{
    /// <summary>
    /// 获取评论的传入参数Dto
    /// </summary>
    public class GetCommentsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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

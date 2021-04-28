using System.ComponentModel.DataAnnotations;
using Abp.Runtime.Validation;
using L._52ABP.Application.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs.Dtos
{
    public class BlogInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
       public long? BlogUserId { get; set; }

       /// <summary>
       /// 当前页
       /// </summary>
       [Range(0, 1000000)]
       public int? CurrentPage { get; set; }

       /// <summary>
       /// 处理数据
       /// </summary>
       public virtual void Normalize()
       {

           if (!this.CurrentPage.HasValue || this.CurrentPage.Value <= 0)
           {
               this.CurrentPage = 1;
           }
           if (this.MaxResultCount <= 0)
           {
               this.MaxResultCount = 10;
           }

           this.SkipCount = (this.CurrentPage.Value - 1) * this.MaxResultCount;
           Sorting = "Id";
        }

    }
}

using System.Collections.Generic;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.AliyunVod.VodCategory.Dtos
{
    public class VodCategoryOutputDto
    {

        /// <summary>
        /// 
        /// </summary>
        public string RequestId { get; set; }
        /// <summary>
        /// 视频分类
        /// </summary>
        public VodCategoryEditDto Category { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public  long? SubTotal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<VodCategoryEditDto> SubCategories { get; set; }

         


    }
  

  
     
}

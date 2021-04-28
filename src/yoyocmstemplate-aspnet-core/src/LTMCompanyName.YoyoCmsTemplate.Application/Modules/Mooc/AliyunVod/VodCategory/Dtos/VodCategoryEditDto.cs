using System.Collections.Generic;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.AliyunVod.VodCategory.Dtos
{
    public class VodCategoryEditDto
    {
        /// <summary>
        /// 真实分类ID
        /// </summary>
        public  long? CateId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public  string CateName  { get; set; }

        public string RequestId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? ParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long? Level { get; set; }

        public string Type{ get; set; }


        public List<VodCategoryEditDto> Children { get; set; }
    }
}

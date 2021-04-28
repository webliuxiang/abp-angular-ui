using System.ComponentModel.DataAnnotations;


namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls.Dtos
{
    /// <summary>
    /// 友情链接的列表DTO
    /// <see cref="Blogroll"/>
    /// </summary>
    public class BlogrollEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// 友情链接分类id
        /// </summary>
        public int BlogrollTypeId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        public string Name { get; set; }



        /// <summary>
        /// Url
        /// </summary>
        [Required(ErrorMessage = "Url不能为空")]
        public string Url { get; set; }



        /// <summary>
        /// 白名单
        /// </summary>
        public bool Except { get; set; }



        /// <summary>
        /// 推荐
        /// </summary>
        public bool Recommend { get; set; }



        /// <summary>
        /// 权重
        /// </summary>
        public int Weight { get; set; }



        /// <summary>
        /// Logo
        /// </summary>
        public string Logo { get; set; }



        /// <summary>
        /// 图标
        /// </summary>
        public string IconName { get; set; }




        //// custom codes



        //// custom codes end
    }
}

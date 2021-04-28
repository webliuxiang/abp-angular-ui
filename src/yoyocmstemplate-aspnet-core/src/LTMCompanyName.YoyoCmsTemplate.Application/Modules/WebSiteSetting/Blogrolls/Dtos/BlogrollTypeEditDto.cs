using System.ComponentModel.DataAnnotations;


namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls.Dtos
{
    /// <summary>
    /// 友情链接分类的列表DTO
    /// <see cref="BlogrollType"/>
    /// </summary>
    public class BlogrollTypeEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public int? Id { get; set; }



        /// <summary>
        /// 分类名称
        /// </summary>
        [MaxLength(100)]
        [Required(ErrorMessage = "分类名称不能为空")]
        public string Name { get; set; }




        //// custom codes



        //// custom codes end
    }
}
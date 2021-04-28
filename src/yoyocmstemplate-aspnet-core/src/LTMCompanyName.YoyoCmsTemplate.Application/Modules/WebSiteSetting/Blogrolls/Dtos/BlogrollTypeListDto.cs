using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;


namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls.Dtos
{
    /// <summary>
    /// 友情链接分类的编辑DTO
    /// <see cref="BlogrollType"/>
    /// </summary>
    public class BlogrollTypeListDto : AuditedEntityDto
    {


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
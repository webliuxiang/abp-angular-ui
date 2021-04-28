using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;


namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.BannerAds.Dtos
{
    /// <summary>
    /// 轮播图广告的编辑DTO
    /// <see cref="BannerAd"/>
    /// </summary>
    public class BannerAdListDto : AuditedEntityDto
    {


        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        public string Title { get; set; }



        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImageUrl { get; set; }



        /// <summary>
        /// 缩略图地址
        /// </summary>
        public string ThumbImgUrl { get; set; }



        /// <summary>
        /// 描述
        /// </summary>
        [Required(ErrorMessage = "描述不能为空")]
        public string Description { get; set; }



        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }



        /// <summary>
        /// 权重
        /// </summary>
        public int Weight { get; set; }



        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }



        /// <summary>
        /// Types
        /// </summary>
        [Required(ErrorMessage = "Types不能为空")]
        public string Types { get; set; }



        /// <summary>
        /// ViewCount
        /// </summary>
        public int ViewCount { get; set; }




        //// custom codes



        //// custom codes end
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.BannerAds
{


    /// <summary>
    /// 轮播图
    /// </summary>
    [Table(AppConsts.TablePrefix + "BannerAds")]

    public class BannerAd : AuditedEntity
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空"), MinLength(10, ErrorMessage = "标题建议设置为10-128字"), MaxLength(128, ErrorMessage = "标题建议设置为10-128字")]
        public string Title { get; set; }

        /// <summary>
        /// 宣传图片
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 小宣传图片
        /// </summary>
        public string ThumbImgUrl { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Required(ErrorMessage = "描述文字不能为空"), MinLength(50, ErrorMessage = "标题建议设置为50-1000字"), MaxLength(1000, ErrorMessage = "标题建议设置为50-1000字")]
        public string Description { get; set; }

        /// <summary>
        /// 广告url
        /// </summary>
        public string Url { get; set; }


        /// <summary>
        /// 权重
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 广告类型
        /// </summary>
        [Required(ErrorMessage = "类型不能为空，至少需要选择一个类型")]
        public string Types { get; set; }

        /// <summary>
        /// 访问次数
        /// </summary>
        public int ViewCount { get; set; }

        public virtual BannerAd IncreaseReadCount()
        {
            ViewCount++;
            return this;
        }
    }
}

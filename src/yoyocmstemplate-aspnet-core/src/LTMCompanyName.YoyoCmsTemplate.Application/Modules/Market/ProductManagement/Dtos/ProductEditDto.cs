using System;
using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.Dtos
{
    public class ProductEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }



        /// <summary>
        /// 产品名称
        /// </summary>
        [Required(ErrorMessage = "请输入产品名称")]
        public string Name { get; set; }



        /// <summary>
        /// 价格
        /// </summary>
        [Required(ErrorMessage = "请输入产品价格")]
        public decimal? Price { get; set; }



        /// <summary>
        /// 产品类型
        /// </summary>
        [Required(ErrorMessage = "产品名称不能为空")]
        public string Type { get; set; }



        /// <summary>
        /// 创建项目次数
        /// </summary>
        [Required(ErrorMessage = "请输入产品创建项目次数")]
        public int? CreateProjectCount { get; set; }



        /// <summary>
        /// 推荐
        /// </summary>
        public bool Recommended { get; set; }



        /// <summary>
        /// 有效天数
        /// </summary>
        public double? Indate { get; set; }



        /// <summary>
        /// 产品助记码,只用来展示,系统自动生成,不可直接编辑
        /// </summary>
        public string Code { get; set; }



        /// <summary>
        /// 是否已发布(已发布的禁止修改)
        /// </summary>
        public bool Published { get; set; }
    }
}

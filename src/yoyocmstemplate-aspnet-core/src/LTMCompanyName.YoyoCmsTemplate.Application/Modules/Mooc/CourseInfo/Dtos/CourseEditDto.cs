using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Enums;

namespace  LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Dtos
{
    public class CourseEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 直播推流地址
        /// </summary>
        public string LivePushUrl { get; set; }

        /// <summary>
        /// 直播拉流地址
        /// </summary>
        public string LivePullUrl { get; set; }
        /// <summary>
        /// 课程简介
        /// </summary>
        public string Intro { get; set; }

        /// <summary>
        /// 课程编码
        /// </summary>
        public string CourseCode { get; set; }

        /// <summary>
        /// 是否为推荐课程
        /// </summary>
        public bool RecommendCourse { get; set; }

        /// <summary>
        /// 难度等级,0-5级
        /// </summary>
        public decimal GradeOfDifficulty { get; set; }


        /// <summary>
        /// 课程标题
        /// </summary>
        [Required(ErrorMessage="课程标题不能为空")]
		public string Title { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public PayTypeEnum PayType { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [Required(ErrorMessage="价格不能为空")]
		public decimal Price { get; set; }



		/// <summary>
		/// 课程显示状态
		/// </summary>
		public CourseTypeEnum Type { get; set; }

        /// <summary>
        /// 课程类型
        /// </summary>
        public CourseVideoTypeEnum CourseVideoType { get; set; }



		/// <summary>
		/// 课程状态
		/// </summary>
		public CourseStateEnum CourseState { get; set; }



		/// <summary>
		/// 封面
		/// </summary>
		public string ImgUrl { get; set; }



		/// <summary>
		/// 课程简介
		/// </summary>
		public string Description { get; set; }



		/// <summary>
		/// 有效天数,0为永久有效
		/// </summary>
		public long ValidDays { get; set; }



		/// <summary>
		/// 直播开始时间
		/// </summary>
		public DateTime? StartTime { get; set; }



        /// <summary>
        /// 购买后的qq群
        /// </summary>
        public string BuyerQqGroup { get; set; }
        /// <summary>
        /// 售前的qq群
        /// </summary>
        public string NotBuyerQqGroup { get; set; }

        /// <summary>
        /// 课程分类
        /// </summary>
        public virtual List<long> CategoryIds { get; set; }

    }
}

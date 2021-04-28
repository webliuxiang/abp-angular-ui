using System;
using System.Collections.Generic;
using Abp.Domain.Entities.Auditing;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Enums;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.Relationships;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo
{
    /// <summary>
    /// 课程
    /// </summary>
    public class Course : FullAuditedEntity<long>
    {
        /// <summary>
        /// 课程标题
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 课程简介
        /// </summary>
        public string Intro { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public virtual decimal Price { get; set; }

        /// <summary>
        /// 课程显示状态
        /// </summary>
        public virtual CourseTypeEnum Type { get; set; }

        /// <summary>
        /// 课程视频类型-直播、录播
        /// </summary>
        public virtual CourseVideoTypeEnum CourseVideoType { get; set; }

        /// <summary>
        /// 课程图片
        /// </summary>
        public virtual string ImgUrl { get; set; }

        /// <summary>
        /// 课程详情
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 课程编码
        /// </summary>
        public string CourseCode { get; set; }
        /// <summary>
        /// 购买后的qq群
        /// </summary>
        public string BuyerQqGroup { get; set; }
        /// <summary>
        /// 售前的qq群
        /// </summary>
        public string NotBuyerQqGroup { get; set; }

        /// <summary>
        /// 是否为推荐课程
        /// </summary>
        public bool RecommendCourse { get; set; }

        /// <summary>
        /// 难度等级,0-5级
        /// </summary>
        public decimal GradeOfDifficulty { get; set; }

        ///// <summary>
        ///// 出售数量
        ///// </summary>
        //public virtual int OrderCount { get; set; }


        /// <summary>
        /// 课程状态
        /// </summary>
        public virtual CourseStateEnum CourseState { get; set; }

        /// <summary>
        /// 有效天数,0为永久有效
        /// </summary>
        public virtual long ValidDays { get; set; }

        /// <summary>
        /// 直播开始时间
        /// </summary>
        public virtual DateTime? StartTime { get; set; }


        public virtual ICollection<CourseToCourseCategory> CourseCategories { get; set; }

        public virtual ICollection<CourseToTeacher> CourseToTeacher { get; set; }

        public virtual ICollection<CourseToStudent> CourseToStudent { get; set; }



        ///// <summary>
        ///// 课程与视频关联表
        ///// </summary>
        //public virtual ICollection<CourseVideoResource> CourseVideoResources { get; set; }



    }
}

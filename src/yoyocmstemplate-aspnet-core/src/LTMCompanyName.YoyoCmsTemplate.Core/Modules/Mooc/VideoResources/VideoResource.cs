using System;
using System.Collections.Generic;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources
{
    /// <summary>
    /// 视频资源
    /// </summary>
    public class VideoResource : Entity<long>
    {
        /// <summary>
        /// 视频名称
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// 视频id
        /// </summary>
        public string VideoId { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        public long? Size { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public decimal? Duration { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string ModificationTime { get; set; }

        /// <summary>
        /// 对应阿里云的CreationTime
        /// </summary>
        public string SyncCreationTime { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        public string CoverURL { get; set; }

        /// <summary>
        /// 分类id
        /// </summary>
        public long? CateId
        {
            get; set;
        }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CateName
        {
            get; set;
        }

        public string DownloadSwitch
        {
            get; set;
        }

        public string TemplateGroupId
        {
            get; set;
        }

        public string PreprocessStatus
        {
            get; set;
        }

        public string StorageLocation
        {
            get; set;
        }

        public string RegionId
        {
            get; set;
        }

        public string CustomMediaInfo
        {
            get; set;
        }

        public string AppId
        {
            get; set;
        }


        public long CreatorUser { get; set; }

    }
}

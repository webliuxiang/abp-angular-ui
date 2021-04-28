
using System;

namespace  LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources.Dtos
{
    public class VideoResourceEditDto
    {
    
        public long? Id { get; set; }


        /// <summary>
        /// 视频名称
        /// </summary>
        public virtual string Title { get; set; }

        public string VideoId { get; set; }

        public string Tags { get; set; }
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

        public string ModificationTime { get; set; }
        /// <summary>
        /// 对应阿里云的CreationTime
        /// </summary>
        public string SyncCreationTime { get; set; }
        /// <summary>
        /// 封面图片
        /// </summary>
        public string CoverURL { get; set; }


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




        /// <summary>
        /// 课程与视频关联表
        /// </summary>
        public long CreatorUser { get; set; }
  
    }
}

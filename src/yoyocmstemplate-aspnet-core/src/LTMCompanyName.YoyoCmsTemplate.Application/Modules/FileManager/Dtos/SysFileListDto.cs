using System;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.FileManager;

namespace LTMCompanyName.YoyoCmsTemplate.SystemBaseManage.Dtos
{
    /// <summary>
    /// 文件的编辑DTO <see cref="SysFile" />
    /// </summary>
    public class SysFileListDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 是否为文件夹
        /// </summary>
        public bool Dir { get; set; }

        /// <summary>
        /// 父级 <see cref="SysFile" /> Id. 如果是根节点，值为null
        /// </summary>
        public virtual Guid? ParentId { get; set; }

        /// <summary>
        /// 是否为图片
        /// </summary>
        public bool IsImg { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 原文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 文件格式
        /// </summary>
        public string FileExt { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 格式化大小
        /// </summary>
        public string FormattedSize { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }

        //// custom codes
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        //// custom codes end
    }
}

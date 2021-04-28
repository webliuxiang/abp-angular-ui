using System;
using LTMCompanyName.YoyoCmsTemplate.Modules.FileManager;

namespace LTMCompanyName.YoyoCmsTemplate.SystemBaseManage.Dtos
{
    /// <summary>
    /// 文件的列表DTO <see cref="SysFile" />
    /// </summary>
    public class SysFileEditDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 是否为文件夹
        /// </summary>
        public bool Dir { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 原文件名
        /// </summary>
        public string FileName { get; set; }

        //// custom codes

        //// custom codes end
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Documents.Dtos
{
    public class GetDocumentInput
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid ProjectId { get; set; }

        [StringLength(ProjectConsts.MaxVersionNameLength)]
        public string Version { get; set; }

        /// <summary>
        /// 文档名称
        /// </summary>
        [Required]
        [StringLength(DocumentConsts.MaxNameLength)]
        public string Name { get; set; }


    }
}

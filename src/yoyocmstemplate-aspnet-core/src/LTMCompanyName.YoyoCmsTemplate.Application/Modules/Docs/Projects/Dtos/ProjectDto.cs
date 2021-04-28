using System;
using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects.Dtos
{
    public class ProjectDto : EntityDto<Guid>
    {
        public string ImgUrl { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string DefaultDocumentName { get; set; }

        public string NavigationDocumentName { get; set; }

        /// <summary>
        /// 最小版本号，用于过滤
        /// </summary>
        public string MinimumVersion { get; set; }

       

        public string LatestVersionBranchName { get; set; }

        public string DocumentStoreType { get; set; }

     

        public string ExtraProperties { get; set; }

        public string MainWebsiteUrl { get; set; }

       

        public virtual string Format { get; set; }

    }
}

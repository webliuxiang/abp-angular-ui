using System;
using System.Collections.Generic;
using Abp;
using Abp.Domain.Entities;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects
{
    public class Project : Entity<Guid>
    {
        /// <summary>
        /// 封面
        /// </summary>
        public virtual string ImgUrl { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        ///要在url中看到的项目的短名称。
        /// </summary>
        public virtual string ShortName { get; set; }

        /// <summary>
        /// 文件的格式(例如:“md”代表Markdown，“html”代表html)。
        /// </summary>
        public virtual string Format { get; set; }

        /// <summary>
        ///初始页面的文档名称
        /// </summary>
        public virtual string DefaultDocumentName { get; set; }

        /// <summary>
        /// 用于导航菜单(索引)的文档。
        /// </summary>
        public virtual string NavigationDocumentName { get; set; }

        /// <summary>
        /// 文件的来源(例如Github)。
        /// </summary>
        public virtual string DocumentStoreType { get; set; }

        public virtual string GoogleCustomSearchId { get; set; }
        /// <summary>
        /// 额外的属性
        /// </summary>
        public virtual string ExtraProperties { get;  set; }

        /// <summary>
        /// 主站的Url
        /// </summary>
        public virtual string MainWebsiteUrl { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool Enabled { get; set; }
        /// <summary>
        /// 排序的Id
        /// </summary>
        public virtual int Sort { get; set; }


        private Dictionary<string, object> _extraProperties;

        /// <summary>
        /// 最后的版本分支信息
        /// </summary>
        public virtual string LatestVersionBranchName { get; set; }
        /// <summary>
        /// 最小版本号
        /// </summary>
        public virtual string MinimumVersion { get; set; }

        /// <summary>
        /// 转换ExtraProperties为字典集合
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetExtraProperties()
        {
            if (_extraProperties==null){
                _extraProperties= JsonConvert.DeserializeObject<Dictionary<string, object>>(ExtraProperties);
            }

            return _extraProperties;


        }



        protected Project()
        {
      

        }

        public Project(Guid id, [NotNull] string name, [NotNull] string shortName, [NotNull] string defaultDocumentName, [NotNull] string navigationDocumentName, string googleCustomSearchId, string mainWebsiteUrl)
        {
            Id = id;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            ShortName = Check.NotNullOrWhiteSpace(shortName, nameof(shortName));
            DefaultDocumentName = Check.NotNullOrWhiteSpace(defaultDocumentName, nameof(defaultDocumentName));
            NavigationDocumentName = Check.NotNullOrWhiteSpace(navigationDocumentName, nameof(navigationDocumentName));
            GoogleCustomSearchId = Check.NotNullOrWhiteSpace(googleCustomSearchId, nameof(googleCustomSearchId));
            MainWebsiteUrl = mainWebsiteUrl;
        }
    }
}

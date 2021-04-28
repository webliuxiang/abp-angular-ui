
using System;

namespace  LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Projects.Dtos
{
    public class ProjectEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }

        public string ImgUrl { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string Name { get; set; }



		/// <summary>
		/// 短名称
		/// </summary>
		public string ShortName { get; set; }

        /// <summary>
        /// 最后的版本分支信息
        /// </summary>
        public virtual string LatestVersionBranchName { get; set; }
        /// <summary>
        /// 最小版本号
        /// </summary>
        public virtual string MinimumVersion { get; set; }


        /// <summary>
        /// 格式化方式
        /// </summary>
        public string Format { get; set; }



		/// <summary>
		/// 默认文档名称
		/// </summary>
		public string DefaultDocumentName { get; set; }



		/// <summary>
		/// 导航文件名称
		/// </summary>
		public string NavigationDocumentName { get; set; }



		/// <summary>
		/// 文档仓储类型
		/// </summary>
		public string DocumentStoreType { get; set; }



		/// <summary>
		/// 扩展属性
		/// </summary>
		public string ExtraProperties { get; set; }



		/// <summary>
		/// 主站点地址
		/// </summary>
		public string MainWebsiteUrl { get; set; }
       


        public  bool Enabled { get; set; }


        /// <summary>
        /// 前台展示优先级，值越大优先级越高
        /// </summary>
        public virtual int Sort { get; set; }
    }
}

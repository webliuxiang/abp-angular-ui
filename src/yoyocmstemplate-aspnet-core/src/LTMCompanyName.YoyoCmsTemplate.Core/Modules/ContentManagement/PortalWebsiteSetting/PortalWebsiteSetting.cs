using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.ContentManagement.PortalWebsiteSetting
{
    /// <summary>
    /// 门户网站设置
    /// </summary>
    public class PortalWebsiteSetting : Entity<long>, IHasCreationTime, IHasModificationTime
    {
        #region 网站基本信息

        /// <summary>
        /// 站点名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 站点logo(图片url
        /// </summary>
        public string Logo { get; set; }
        /// <summary>
        /// url栏图标(图片url
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// Site Root Url
        /// </summary>
        public string UrlAddress { get; set; }
        /// <summary>
        /// 站点关键字,|线分隔
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 网站描述，
        /// 一般显示在搜索引擎搜索结果中的描述文字，
        /// 用于介绍网站，吸引浏览者点击。(100字以内)
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 版权信息
        /// </summary>
        public string Copyright { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 其它信息
        /// </summary>
        public string OtherInfo { get; set; }
        /// <summary>
        /// 自定义页面顶部代码段(前台
        /// </summary>
        public string TopCustomCodeSnippet { get; set; }
        /// <summary>
        /// 自定义页面底部的代码段(前台
        /// </summary>
        public string BottomCustomCodeSnippet { get; set; }

       

        #endregion


        #region Email

        /// <summary>
        /// 邮箱发件人
        /// </summary>
        public string Addresser { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string MailAddress { get; set; }

        /// <summary>
        /// 邮箱密码
        /// </summary>
        public string MailPassword { get; set; }

        /// <summary>
        /// SMTP Host
        /// </summary>
        public string SMTP { get; set; }

        /// <summary>
        /// 发送端口
        /// </summary>
        public int SendPort { get; set; }

        /// <summary>
        /// 发送方式
        /// </summary>
        public EmailSendTypeEnum SendType { get; set; }

        #endregion


        #region 关于我们 HTML内容

      
        /// <summary>
        /// 关于我们（富文本）
        /// </summary>
        public string AboutUsContent { get; set; }
        #endregion

        public DateTime? LastModificationTime { get; set; }
        public DateTime CreationTime { get; set; }
        
    }
}

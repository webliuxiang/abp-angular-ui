using Abp.Dependency;

namespace LTMCompanyName.YoyoCmsTemplate.AppFolders
{
    public class AppFolder : IAppFolder, ISingletonDependency
    {
        /// <summary>
        /// 临时文件下载目录
        /// </summary>
        public string TempFileDownloadFolder { get; set; }

        public string SampleProfileImagesFolder { get; set; }

        /// <summary>
        /// 文件管理的根目录
        /// </summary>
        public string SysFileRootFolder { get; set; }

        /// <summary>
        /// 文档管理（Wiki）的根目录
        /// </summary>
        public string ProjectRootFolder { get; set; }

        /// <summary>
        /// 站点日志文件夹
        /// </summary>
        public string WebSiteLogsFolder { get; set; }

        public string TempFileInfoFolder { get; set; }

        public string OfficialFileInfoFolder { get; set; }

        public string TempResourceItemFolder { get; set; }

        public string OfficialResourceItemFolder { get; set; }

        public string TempSuppleyAndDemmandFolder { get; set; }

        public string OfficialSuppleyAndDemmandFolder { get; set; }

        public string TempResourceItemImg { get; set; }

        public string OfficialResourceItemImg { get; set; }

        public string CarouselPicturePath { get; set; }

        public string CarouselPictureTempPath { get; set; }

        public string PartnerPicturePath { get; set; }

        /// <summary>
        /// 商品详情图片文件夹
        /// </summary>
        public string CommodityDetailItemFolder { get; set; }

        public string WebContentRootPath { get; set; }


        #region ABP项目模板
        /// <summary>
        /// 下载ABP模板
        /// </summary>
        public string DownloadAbpTemplate { get; set; }


        /// <summary>
        /// 下载付费项目文件夹
        /// </summary>

        public string DownloadProjectChargeDirectory { get; set; }
        /// <summary>
        /// 下载免费项目文件夹
        /// </summary>
        public string DownloadProjectFreeDirectory { get; set; }


        #endregion

    }
}

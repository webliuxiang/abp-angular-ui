using System;
using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.Dtos
{
    public class UserDownloadConfigListDto : EntityDto<long>
    {


        /// <summary>
        /// 产品助记码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 下载类型
        /// </summary>
        public string DownloadType { get; set; }



        /// <summary>
        /// 下载次数
        /// </summary>
        public long ResidueDegree { get; set; }



        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }



        /// <summary>
        /// 有效天数
        /// </summary>
        public long Indate { get; set; }


        public string UserName { get; set; }




    }
}

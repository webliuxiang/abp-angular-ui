using System;
using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.Dtos
{
    public class UserDownloadConfigEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }



        /// <summary>
        /// 下载类型
        /// </summary>
        public string DownloadType { get; set; }

        /// <summary>
        /// 产品助记码
        /// </summary>
        [Required(ErrorMessage = "产品不能为空")]
        public string ProductCode { get; set; }


        /// <summary>
        /// 下载次数
        /// </summary>
        [Required(ErrorMessage = "下载次数不能为空")]
        public long ResidueDegree { get; set; }



        /// <summary>
        /// 开始时间
        /// </summary>
        [Required(ErrorMessage = "开始时间不能为空")]
        public DateTime? StartTime { get; set; }



        /// <summary>
        /// 有效天数
        /// </summary>
        [Required(ErrorMessage = "有效天数不能为空")]
        public long Indate { get; set; }


        [Required(ErrorMessage = "UserName不能为空")]
        public string UserName { get; set; }




    }
}

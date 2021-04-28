using System;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.AliyunVod.VodUploads.Dto
{
    /*
     * https://help.aliyun.com/document_detail/55636.html?spm=a2c4g.11186623.4.4.5bd54b63rrgl1h
     */

    public class AliCallBackInput
    {
        public string VideoId { get; set; }

        public string EventType { get; set; }

        public DateTime EventTime { get; set; }

        public long Size { get; set; }

        public string FileUrl { get; set; }
    }
}

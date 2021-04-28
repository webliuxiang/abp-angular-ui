using System;
using Abp.Web.Models;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.UploadFiles.Dtos
{
    public class UploadFileOutputDto : ErrorInfo
    {
        public UploadFileOutputDto()
        {
        }

        public UploadFileOutputDto(ErrorInfo error)
        {
            Code = error.Code;
            Details = error.Details;
            Message = error.Message;
            ValidationErrors = error.ValidationErrors;
        }

        /// <summary>
        /// 文件Id
        /// </summary>
        public Guid? FileId { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
    }
}

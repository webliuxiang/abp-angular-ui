using System;
using Abp.Web.Models;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.UploadFiles.Dtos
{
    /// <summary>
    /// 处理个人文件上传的错误信息呈现
    /// </summary>
    public class UploadProfilePictureOutputDto : ErrorInfo
    {
        public UploadProfilePictureOutputDto()
        {
        }

        public UploadProfilePictureOutputDto(ErrorInfo error)
        {
            Code = error.Code;
            Details = error.Details;
            Message = error.Message;
            ValidationErrors = error.ValidationErrors;
        }

        /// <summary>
        /// </summary>
        public Guid? ProfilePictureId { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public string FileToken { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}

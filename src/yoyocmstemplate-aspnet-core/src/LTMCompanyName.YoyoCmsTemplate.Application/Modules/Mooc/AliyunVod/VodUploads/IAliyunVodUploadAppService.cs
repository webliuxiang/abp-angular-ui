using System.Threading.Tasks;
using Abp.Application.Services;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.AliyunVod.VodUploads.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.AliyunVod.VodUploads
{
    public interface IAliyunVodUploadAppService : IApplicationService
    {



        /// <summary>
        /// 创建视频上传凭证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<VideoAutherOutput>  CreateUploadVideoRequestAsync(VodUploadDto input);

        /// <summary>
        /// 刷新视频上传凭证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        VideoAutherOutput RefreshUploadVideoRequest(RefreshUploadInputDto input);

       
    }
}

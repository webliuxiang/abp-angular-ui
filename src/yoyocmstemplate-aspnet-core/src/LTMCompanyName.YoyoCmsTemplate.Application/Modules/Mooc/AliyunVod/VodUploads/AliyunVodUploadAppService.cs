using System;
using Abp.Domain.Repositories;
using Abp.UI;
using Abp.Web.Models;
using Aliyun.Acs.vod.Model.V20170321;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.AliyunVod.VodUploads.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources.DomainService;
using Mren.Aliyun;
using Yoyo.Abp.Vod;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.AliyunVod.VodUploads
{
    public class AliyunVodUploadAppService : IAliyunVodUploadAppService
    {
        #region 初始化

        private readonly AliyunVodManager _aliyunVodManager;
        private IRepository<VideoResource, long> _videoResourceRepository;

         public AliyunVodUploadAppService(AliyunVodManager aliyunVodManager, IRepository<VideoResource, long> videoResourceRepository)
         {
             _aliyunVodManager = aliyunVodManager;
             _videoResourceRepository = videoResourceRepository;
         }

        #endregion

        #region  上传

        public async System.Threading.Tasks.Task<VideoAutherOutput> CreateUploadVideoRequestAsync(VodUploadDto input)
        {
            CreateUploadVideoRequest request =
                new CreateUploadVideoRequest {Title = input.Title, FileName = input.FileName,CateId = input.CateId};

          var videoInfo=await  _videoResourceRepository.FirstOrDefaultAsync(a => a.CateName == input.Title && a.CateId == input.CateId);
          if (videoInfo!=null)
          {
              throw  new UserFriendlyException($"{videoInfo.CateName}下已经有包含的同名{input.Title},请重命名后再次上传");
          }


 
            var vodClient = _aliyunVodManager.InitVodClient();

            CreateUploadVideoResponse data = vodClient.GetAcsResponse(request);

            var response = new VideoAutherOutput() { UploadAuth = data.UploadAuth, VideoId = data.VideoId, UploadAddress = data.UploadAddress };

            return response;
        }

        public VideoAutherOutput RefreshUploadVideoRequest(RefreshUploadInputDto input)
        {
            try
            {

                RefreshUploadVideoRequest request = new RefreshUploadVideoRequest();
                request.VideoId = input.VideoId;

                var vodClient = _aliyunVodManager.InitVodClient();

                RefreshUploadVideoResponse data = vodClient.GetAcsResponse(request);

                var response = new VideoAutherOutput() { UploadAuth = data.UploadAuth, VideoId=data.VideoId, UploadAddress=data.UploadAddress };

                return response;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        #endregion

      
    }
}

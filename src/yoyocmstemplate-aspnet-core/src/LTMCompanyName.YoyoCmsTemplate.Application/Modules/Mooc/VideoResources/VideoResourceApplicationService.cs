using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Aliyun.Acs.Core;
using Aliyun.Acs.vod.Model.V20170321;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Yoyo.Abp.Vod;
using Yoyo.Abp.Vod.Enum;
using Abp.Extensions;
using LTMCompanyName.YoyoCmsTemplate.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources
{
    /// <summary>
    ///     VideoResource应用层服务的接口实现方法
    /// </summary>
    [AbpAuthorize]
    public class VideoResourceAppService : YoyoCmsTemplateAppServiceBase, IVideoResourceAppService
    {
        private readonly AliyunVodManager _aliyunVodManager;

        private readonly IVideoResourceManager _videoResourceManager;

        private readonly IRepository<VideoResource, long> _videoResourceRepo;

        /// <summary>
        ///     构造函数
        /// </summary>
        public VideoResourceAppService(
            IRepository<VideoResource, long> videoResourceRepository,
            IVideoResourceManager videoResourceManager,
            AliyunVodManager aliyunVodManager
            )
        {
            _videoResourceRepo = videoResourceRepository;
            _videoResourceManager = videoResourceManager;
            _aliyunVodManager = aliyunVodManager;
        }

        /// <summary>
        ///     获取VideoResource的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(VideoResourcePermissions.Query)]
        [HttpPost]
        public async Task<PagedResultDto<VideoResourceListDto>> GetPaged(QueryInput input)
        {
            var query = _videoResourceRepo.GetAll()
                .AsNoTracking()
                .Where(input.QueryConditions);

            var count = await query.CountAsync();

            var entityList = await query
                .OrderBy(input.SortConditions)
                .PageBy(input)
                .ToListAsync();

            var entityListDtos = ObjectMapper.Map<List<VideoResourceListDto>>(entityList);

            return new PagedResultDto<VideoResourceListDto>(count, entityListDtos);
        }

        /// <summary>
        ///     通过指定id获取VideoResourceListDto信息
        /// </summary>
        [AbpAuthorize(VideoResourcePermissions.Query)]
        public async Task<VideoResourceListDto> GetById(EntityDto<long> input)
        {
            var entity = await _videoResourceRepo.GetAsync(input.Id);

            return ObjectMapper.Map<VideoResourceListDto>(entity);



        }

        /// <summary>
        ///     获取编辑 VideoResource
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(VideoResourcePermissions.Create, VideoResourcePermissions.Edit)]
        public async Task<GetVideoResourceForEditOutput> GetForEdit(NullableIdDto<long> input)
        {
            var output = new GetVideoResourceForEditOutput();
            VideoResourceEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _videoResourceRepo.GetAsync(input.Id.Value);


                editDto = ObjectMapper.Map<VideoResourceEditDto>(entity);
            }
            else
            {
                editDto = new VideoResourceEditDto();
            }

            output.VideoResource = editDto;
            return output;
        }

        /// <summary>
        ///     添加或者修改VideoResource的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(VideoResourcePermissions.Create, VideoResourcePermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateVideoResourceInput input)
        {
            if (input.VideoResource.Id.HasValue)
            {
                await Update(input.VideoResource);
            }
            else
            {
                throw new UserFriendlyException("静止创建空视频内容");
#pragma warning disable CS0162 // 检测到无法访问的代码
                await Create(input.VideoResource);
#pragma warning restore CS0162 // 检测到无法访问的代码
            }
        }

        /// <summary>
        ///     删除VideoResource信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(VideoResourcePermissions.Delete)]
        public async Task Delete(EntityDto<string> input)
        {


            //TODO:删除前的逻辑判断，是否允许删除
            await _videoResourceRepo.DeleteAsync(a => a.VideoId == input.Id);

            try
            {
                var res = _aliyunVodManager.DeleteAliyunVodInfo(new DeleteVideoRequest()
                {
                    VideoIds = input.Id
                });
            }
            catch (Exception e)
            {
                if (e.Message.Contains("InvalidVideo.NotFound"))
                {

                }
                else
                {
                    Console.WriteLine(e);
                    throw;
                }

            }





        }

        /// <summary>
        ///     批量删除VideoResource的方法
        /// </summary>
        [AbpAuthorize(VideoResourcePermissions.BatchDelete)]
        public async Task BatchDelete(List<string> input)
        {

            var ids = string.Join(",", input);

            try
            {

                _aliyunVodManager.DeleteAliyunVodInfo(new DeleteVideoRequest()
                {
                    VideoIds = ids
                });
            }
            catch (Exception e)
            {
                if (e.Message.Contains("InvalidVideo.NotFound"))
                {

                }
                else
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
            await _videoResourceRepo.DeleteAsync(s => input.Contains(s.VideoId));
        }

        public dynamic GetVodTest()
        {
            // 构造请求
            var request = new GetCategoriesRequest
            {
                // 分类ID，若不填，则默认获取根节点及其子分类，根节点分类ID为-1
                CateId = -1,
                PageNo = 1,
                PageSize = 10
            };

            var dto = _aliyunVodManager.GetMediaCategories(request);

            //var videoid = "1ee3c18b52234f6f8189d7a91be4e822";
            //1ee3c18b52234f6f8189d7a91be4e822
            //  var input = new GetCategoriesRequest() { CateId = -1, PageNo = 1, PageSize = 10  };
            //       // 循序渐进掌握 ASP.NET Core 与 EntityFramework Core
            //var dto = _aliyunVodManager.GetMediaCategories(input);
            //SearchMediaRequest request = new SearchMediaRequest();
            //request.SearchType = AliyunVodAppConts.MediaTypeConsts.Video;
            //request.Fields = AliyunVodAppConts.SearchMediaCommonFilds.VideoCommonFilds;

            //request.PageNo = 1;
            //request.PageSize = 10;
            //GetVideoInfoRequest request = new GetVideoInfoRequest();
            //request.VideoId = videoid;

            //var dto  =  _aliyunVodManager.GetVideoInfo(request);

            //var request = new SearchMediaRequest
            //{
            //    SearchType = AliyunVodAppConts.MediaTypeConsts.Video,
            //    PageNo = 1,
            //    PageSize = 20,
            //    Fields = "CreateTime"
            //};

            //var dto = _aliyunVodManager.SearchMediaList(request);

            return dto;
        }

        /// <summary>
        ///     同步阿里云视频
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(VideoResourcePermissions.SynchronizeAliyunVodAsync)]
        public async Task<dynamic> SynchronizeAliyunVodAsync()
        {
            var en = await _videoResourceManager.SynchronousVideoVodData();

            await _videoResourceManager.SychronousVideoInsertToData(en);

            return en;
        }

        public async Task<VideoPlayAuthDto> GetVideoResourcePlayAuth(EntityDto<string> videoInput)
        {
            var entity = await _videoResourceRepo.FirstOrDefaultAsync(a => a.VideoId == videoInput.Id);

            var input = new GetVideoPlayAuthRequest();

            if (entity.Duration.HasValue && entity.Duration < 3000)
            {
                var duration = Convert.ToInt32(entity.Duration.Value);
                input.AuthInfoTimeout = duration;
            }
            else
            {
                input.AuthInfoTimeout = 3000;
            }

            input.VideoId = entity.VideoId;

            var res = _aliyunVodManager.GetVideoPlayAuth(input);

            var dto = new VideoPlayAuthDto
            {
                CoverURL = res.VideoMeta.CoverURL,
                VideoId = res.VideoMeta.VideoId,
                PlayAuth = res.PlayAuth
            };

            //dto.VideoMeta.Title
            //dto.VideoMeta.VideoId
            //     dto.PlayAuth 50分钟。

            return dto;
        }

        /// <summary>
        ///     新增VideoResource
        /// </summary>
        [AbpAuthorize(VideoResourcePermissions.Create)]
        protected virtual async Task<VideoResourceEditDto> Create(VideoResourceEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<VideoResource>(input);
            //        var entity = input.MapTo<VideoResource>();

            entity = await _videoResourceRepo.InsertAsync(entity);

            var dto = ObjectMapper.Map<VideoResourceEditDto>(entity);
            return dto;
        }

        /// <summary>
        ///     编辑VideoResource
        /// </summary>
        [AbpAuthorize(VideoResourcePermissions.Edit)]
        protected virtual async Task Update(VideoResourceEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _videoResourceRepo.GetAsync(input.Id.Value);
            //       input.MapTo(entity);

            // 构造请求
            var request = new UpdateVideoInfoRequest
            {
                VideoId = input.VideoId,
                Title = input.Title,
                Description = input.Description
            };
            // 初始化客户端
            var vodClient = _aliyunVodManager.InitVodClient();

            // 发起请求，并得到 response
            var response = vodClient.GetAcsResponse(request);


            ObjectMapper.Map(input, entity);
            await _videoResourceRepo.UpdateAsync(entity);
        }

        [HttpPost]
        public async Task<ListResultDto<VideoResourceListDto>> SearchVideoResources(SearchVideoResourceInput input)
        {
            var entityList = await _videoResourceRepo.GetAll()
                .AsNoTracking()
                .Where(o => o.CateId.HasValue && o.CateId.Value == input.VideoCategoryId)
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(), o => o.Title.Contains(input.Filter))
                .ToListAsync();


            return new ListResultDto<VideoResourceListDto>()
            {
                Items = ObjectMapper.Map<IReadOnlyList<VideoResourceListDto>>(entityList)
            };
        }
    }
}

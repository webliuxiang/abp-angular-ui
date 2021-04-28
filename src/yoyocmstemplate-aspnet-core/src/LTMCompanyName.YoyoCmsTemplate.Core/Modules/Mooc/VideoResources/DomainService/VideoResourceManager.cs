using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Abp.Domain.Repositories;

using Aliyun.Acs.vod.Model.V20170321;

using Microsoft.EntityFrameworkCore;

using Yoyo.Abp.Vod;
using Yoyo.Abp.Vod.Enum;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.VideoResources.DomainService
{
    /// <summary>
    ///     VideoResource领域层的业务管理
    /// </summary>
    public class VideoResourceManager : AbpDomainService<VideoResource>, IVideoResourceManager
    {
        private readonly AliyunVodManager _aliyunVodManager;


        /// <summary>
        ///     VideoResource的构造方法
        /// </summary>
        public VideoResourceManager(
                IRepository<VideoResource, long> repository,
                AliyunVodManager aliyunVodManager
            )
            : base(repository)
        {
            _aliyunVodManager = aliyunVodManager;
        }

        /// <summary>
        ///     初始化
        /// </summary>
        public string InitVideoResource()
        {
            var str =
                _aliyunVodManager.InitMethod();

            return str;
        }

        public async Task<VideoResource> GetLastVideoResource()
        {
            return await QueryAsNoTracking.LastOrDefaultAsync();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<List<VideoResource>> SynchronousVideoVodData()
        {
            await Task.Yield();

            var dtos = new GetVideoInfosResponse
            {
                VideoList = new List<GetVideoInfosResponse.GetVideoInfos_Video>()
            };
            var maxResult = 20; //批量查询vid不能超过20条
            var request = new SearchMediaRequest
            {
                SearchType = AliyunVodAppConts.MediaTypeConsts.Video,
                PageNo = 1,
                PageSize = maxResult,
                Fields = "CreateTime"
            };

            var media = _aliyunVodManager.SearchMediaList(request);
            var totalRecord = media.Total; //2

            if (totalRecord < maxResult)
            {
                var vidList = media.MediaList.Select(a => a.Video.VideoId).ToList();
                dtos = GetVodVideoBatchVideosInfo(vidList);

                return ConvertToVideoResources(dtos.VideoList);
            }

            //大于了2页数据
            //获取总页数
            var totalPage = (totalRecord + maxResult - 1) / maxResult; // 3

            //模拟分页获取数据
            for (var i = 0; i < totalPage; i++)
            {
                request.PageNo = i + 1; //1+1

                var mediaList = _aliyunVodManager.SearchMediaList(request);
                var mediaIds = mediaList.MediaList.Select(a => a.Video.VideoId).ToList();

                var subDtos = GetVodVideoBatchVideosInfo(mediaIds);

                dtos.VideoList.AddRange(subDtos.VideoList);
            }

            return ConvertToVideoResources(dtos.VideoList);
        }

        /// <summary>
        ///     更加旧数据，添加新数据到数据库中
        /// </summary>
        public async Task SychronousVideoInsertToData(List<VideoResource> newVideoInfos)
        {
            var newVideoIds = newVideoInfos.Select(a => a.VideoId).ToList();

            //已经存在的数据,
            var dataVideoInfos = await QueryAsNoTracking
                .Where(a => newVideoIds.Contains(a.VideoId)).ToListAsync();

            //已经存在的数据，做数据更新处理
            var updateVideos = newVideoInfos.Where(a => dataVideoInfos.Exists(d => d.VideoId == a.VideoId)).ToList();

            updateVideos.ForEach(o => newVideoInfos.Remove(o));

            foreach (var newVideoInfo in newVideoInfos)
            {
                await EntityRepo.InsertAsync(newVideoInfo);
            }

            foreach (var entity in updateVideos)
            {
                //更新数据中的数据
                var tempEntity = dataVideoInfos.Find(o => o.VideoId == entity.VideoId);

                if (tempEntity == null)
                {
                    continue;
                }

                var id = tempEntity.Id;

                tempEntity = entity;

                tempEntity.Id = id;

                await UpdateAsync(tempEntity);
            }
        }

        private List<VideoResource> ConvertToVideoResources(List<GetVideoInfosResponse.GetVideoInfos_Video> videoInfos)
        {
            var models = ObjectMapper.Map<List<VideoResource>>(videoInfos);

            return models;

            //dto.Title
        }

        private async Task UpdateAsync(VideoResource entity)
        {
            await EntityRepo.UpdateAsync(entity);
        }

        /// <summary>
        ///     批量获取视频信息
        /// </summary>
        /// <param name="vids"></param>
        /// <returns></returns>
        public GetVideoInfosResponse GetVodVideoBatchVideosInfo(List<string> vids)
        {
            if (vids.Count < 1)
            {
                return new GetVideoInfosResponse();
            }

            var vidList = string.Join(",", vids);

            var input = new GetVideoInfosRequest { VideoIds = vidList };

            var response = _aliyunVodManager.GetBatchVideosInfo(input);

            return response;

#pragma warning disable CS0162 // 检测到无法访问的代码
            //返回不存在的videoID

            if (response.NonExistVideoIds != null && response.NonExistVideoIds.Count > 0)
            {
                foreach (var videoId in response.NonExistVideoIds)
                {
                    Console.WriteLine("NonExist videoId = " + videoId);
                }
            }

            //返回存在的Video信息
            if (response.VideoList != null && response.VideoList.Count > 0)
            {
                foreach (var video in response.VideoList)
                {
                    Console.WriteLine("MediaId = " + video.VideoId);
                    Console.WriteLine("Title = " + video.Title);
                    Console.WriteLine("CreationTime = " + video.CreationTime);
                    Console.WriteLine("CoverURL = " + video.CoverURL);
                    Console.WriteLine("Status = " + video.Status);
                }
            }
#pragma warning restore CS0162 // 检测到无法访问的代码
        }
        #region 自定义权限验证




        #endregion
        // TODO:编写领域业务代码
    }
}

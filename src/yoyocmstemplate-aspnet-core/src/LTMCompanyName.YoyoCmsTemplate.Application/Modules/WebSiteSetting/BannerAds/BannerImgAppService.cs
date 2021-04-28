
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.BannerAds.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.BannerAds.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.BannerAds
{
    /// <summary>
    /// 轮播图广告应用层服务的接口实现方法
    ///</summary>
    public class BannerImgAppService : YoyoCmsTemplateAppServiceBase, IBannerImgAppService
    {
        private readonly IRepository<BannerAd, int> _bannerAdRepository;



        private readonly IBannerAdManager _bannerAdManager;
        /// <summary>
        /// 构造函数
        ///</summary>
        public BannerImgAppService(
        IRepository<BannerAd, int> bannerAdRepository
              , IBannerAdManager bannerAdManager

             )
        {
            _bannerAdRepository = bannerAdRepository;
            _bannerAdManager = bannerAdManager; ;


        }


        /// <summary>
        /// 获取轮播图广告的分页列表信息
        ///      </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BannerAdPermissions.Query)]
        public async Task<PagedResultDto<BannerAdListDto>> GetPaged(GetBannerAdsInput input)
        {

            var query = _bannerAdRepository.GetAll()

                          //模糊搜索标题
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Title.Contains(input.FilterText))
                          //模糊搜索图片地址
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.ImageUrl.Contains(input.FilterText))
                          //模糊搜索缩略图地址
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.ThumbImgUrl.Contains(input.FilterText))
                          //模糊搜索描述
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Description.Contains(input.FilterText))
                          //模糊搜索Url
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Url.Contains(input.FilterText))
                          //模糊搜索Types
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Types.Contains(input.FilterText))
            ;
            // TODO:根据传入的参数添加过滤条件

            var count = await query.CountAsync();

            var bannerAdList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            var bannerAdListDtos = ObjectMapper.Map<List<BannerAdListDto>>(bannerAdList);

            return new PagedResultDto<BannerAdListDto>(count, bannerAdListDtos);
        }


        /// <summary>
        /// 通过指定id获取BannerAdListDto信息
        /// </summary>
        [AbpAuthorize(BannerAdPermissions.Query)]
        public async Task<BannerAdListDto> GetById(EntityDto<int> input)
        {
            var entity = await _bannerAdRepository.GetAsync(input.Id);

            var dto = ObjectMapper.Map<BannerAdListDto>(entity);
            return dto;
        }

        /// <summary>
        /// 获取编辑 轮播图广告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BannerAdPermissions.Create, BannerAdPermissions.Edit)]
        public async Task<GetBannerAdForEditOutput> GetForEdit(NullableIdDto<int> input)
        {
            var output = new GetBannerAdForEditOutput();
            BannerAdEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _bannerAdRepository.GetAsync(input.Id.Value);
                editDto = ObjectMapper.Map<BannerAdEditDto>(entity);
            }
            else
            {
                editDto = new BannerAdEditDto();
            }



            output.BannerAd = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改轮播图广告的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BannerAdPermissions.Create, BannerAdPermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateBannerAdInput input)
        {

            if (input.BannerAd.Id.HasValue)
            {
                await Update(input.BannerAd);
            }
            else
            {
                await Create(input.BannerAd);
            }
        }


        /// <summary>
        /// 新增轮播图广告
        /// </summary>
        [AbpAuthorize(BannerAdPermissions.Create)]
        protected virtual async Task<BannerAdEditDto> Create(BannerAdEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<BannerAd>(input);
            //调用领域服务
            entity = await _bannerAdManager.CreateAsync(entity);

            var dto = ObjectMapper.Map<BannerAdEditDto>(entity);
            return dto;
        }

        /// <summary>
        /// 编辑轮播图广告
        /// </summary>
        [AbpAuthorize(BannerAdPermissions.Edit)]
        protected virtual async Task Update(BannerAdEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _bannerAdRepository.GetAsync(input.Id.Value);
            //  input.MapTo(entity);
            //将input属性的值赋值到entity中
            ObjectMapper.Map(input, entity);
            await _bannerAdManager.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除轮播图广告信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BannerAdPermissions.Delete)]
        public async Task Delete(EntityDto<int> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _bannerAdManager.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除BannerAd的方法
        /// </summary>
        [AbpAuthorize(BannerAdPermissions.BatchDelete)]
        public async Task BatchDelete(List<int> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _bannerAdManager.BatchDelete(input);
        }
        /// <summary>
        /// 获取前端门户使用的banner图
        /// </summary>
        /// <returns></returns>
        public async Task<List<BannerAdListDto>> GetForReadBannerAds()
        {
            var banners = await _bannerAdManager.QueryBannerAdsAsNoTracking().OrderByDescending(a => a.Weight).ThenByDescending(a => a.Price).Take(5)
                .ToListAsync();

            //foreach (var item in banners)
            //{


            //}

            var dtos = ObjectMapper.Map<List<BannerAdListDto>>(banners);
            return dtos;

        }


        //// custom codes



        //// custom codes end

    }
}



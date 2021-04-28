using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Backgrounds.BackgroundWorkers;
using LTMCompanyName.YoyoCmsTemplate.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders
{
    /// <summary>
    /// OrderInfo应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class NeteaseOrderInfoAppService : YoyoCmsTemplateAppServiceBase, INeteaseOrderInfoAppService
    {
        private readonly IRepository<NeteaseOrders.NeteaseOrderInfo, long> _neteaseOrderRepository;

        private readonly INeteaseOrderInfoManager _neteaseOrderManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public NeteaseOrderInfoAppService(
            IRepository<NeteaseOrders.NeteaseOrderInfo, long> neteaseOrderRepository,
            INeteaseOrderInfoManager neteaseOrderManager
        )
        {
            _neteaseOrderRepository = neteaseOrderRepository;
            _neteaseOrderManager = neteaseOrderManager;
        }


        /// <summary>
        /// 获取OrderInfo的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[AbpAuthorize(NeteaseOrderInfoPermissions.Query)]
        [HttpPost]
        public async Task<PagedResultDto<NeteaseOrderInfoListDto>> GetPaged(QueryInput input)
        {
            var query = _neteaseOrderRepository.GetAll()
                .AsNoTracking()
               .Where(input.QueryConditions);
            // TODO:根据传入的参数添加过滤条件
 
            var count = await query.CountAsync();

            var entityList = await query
                .OrderBy(input.SortConditions)
                .PageBy(input)
                .ToListAsync(); 

            var entityListDtos = ObjectMapper.Map<List<NeteaseOrderInfoListDto>>(entityList);
 

            return new PagedResultDto<NeteaseOrderInfoListDto>(count, entityListDtos);
        }



        /// <summary>
        /// 获取 OrderInfo 详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(NeteaseOrderInfoPermissions.Query)]
        public async Task<NeteaseOrderInfoEditDto> GetDetailsById(NullableIdDto<long> input)
        {

            if (input.Id.HasValue)
            {
                var entity = await _neteaseOrderRepository.GetAsync(input.Id.Value);

                return ObjectMapper.Map<NeteaseOrderInfoEditDto>(entity);

             }
            else
            {
                throw new UserFriendlyException("查询数据错误!");
            }

        }

        #region 审核状态

        [AbpAuthorize(NeteaseOrderInfoPermissions.Edit)]
        public async Task UpdateChecked(NullableIdDto<long> input, bool isChecked)
        {
            if (input.Id.HasValue)
            {
                await _neteaseOrderManager.UpdateChecked(input.Id.Value, isChecked);
            }
            else
            {
                throw new UserFriendlyException("提交数据错误!");
            }
        }

        [AbpAuthorize(NeteaseOrderInfoPermissions.Edit)]
        public async Task BatchUpdateChecked(List<long> input, bool isChecked)
        {
            await _neteaseOrderManager.UpdateChecked(input, isChecked);
        }


        [AbpAuthorize(NeteaseOrderInfoPermissions.Edit)]
        public async Task UpdateGiteeChecked(NullableIdDto<long> input, bool isChecked)
        {
            if (input.Id.HasValue)
            {
                await _neteaseOrderManager.UpdateGiteeChecked(input.Id.Value, isChecked);
            }
            else
            {
                throw new UserFriendlyException("提交数据错误!");
            }
        }

        [AbpAuthorize(NeteaseOrderInfoPermissions.Edit)]
        public async Task BatchUpdateGiteeChecked(List<long> input, bool isChecked)
        {
            await _neteaseOrderManager.UpdateGiteeChecked(input, isChecked);
        }


        #endregion



        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(NeteaseOrderInfoPermissions.Edit)]
        [UnitOfWork(isTransactional: false)]
        public async Task RefreshOrderInfo(NullableIdDto<long> input)
        {
            var entity = await _neteaseOrderManager.GetOrderById(input.Id.Value);

            var orderInfos = await CloudStudyOrderInfoWorker.ExecuteAsync(new CloudStudyOrderInfoEventArgs()
            {
                StartDate = entity.OrderDate.Value.AddDays(-1),
                EndDate = entity.OrderDate.Value.AddDays(1)
            });
            await _neteaseOrderManager.Create(orderInfos, CloudStudyOrderInfoWorker.platform);

        }

        [AbpAuthorize(NeteaseOrderInfoPermissions.Query)]
        public async Task<ListResultDto<NeteaseOrderInfoStatisticsDto>> GetStatistics()
        {
            var query = _neteaseOrderRepository.GetAll().AsNoTracking().Where(o => o.TransactionStatus == "交易成功");

            var restList = await (from a in query
                                  group a by a.ProductName into g
                                  select new NeteaseOrderInfoStatisticsDto
                                  {
                                      Key = g.Key,// 课程名称
                                      Value = g.Sum(o => o.RealityAmount),// 扣除手续费实际收入
                                      Sum = g.Sum(o => o.TransactionAmount),// 交易金额
                                      BuyCount = g.Count()
                                  }).ToListAsync();

            return new ListResultDto<NeteaseOrderInfoStatisticsDto>(restList);
        }

        [AbpAuthorize(NeteaseOrderInfoPermissions.Edit)]
        [UnitOfWork(isTransactional: false)]
        public async Task Synchronize()
        {
            var orderInfos = await CloudStudyOrderInfoWorker.ExecuteAsync(new CloudStudyOrderInfoEventArgs()
            {

            });
            await _neteaseOrderManager.Create(orderInfos, CloudStudyOrderInfoWorker.platform);
        }


    }
}



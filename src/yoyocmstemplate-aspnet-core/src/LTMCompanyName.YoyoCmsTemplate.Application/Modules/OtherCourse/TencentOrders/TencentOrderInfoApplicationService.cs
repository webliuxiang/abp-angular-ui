using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using LTMCompanyName.YoyoCmsTemplate.Extension;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.Excel.Importing.Dto;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders
{
    /// <summary>
    ///     TencentOrderInfo应用层服务的接口实现方法
    /// </summary>
    [AbpAuthorize]
    public class TencentOrderInfoAppService : YoyoCmsTemplateAppServiceBase, ITencentOrderInfoAppService
    {
        private readonly IRepository<BackgroundJobInfo, long> _backgroundJobRepository;
        private readonly DataFileObjectManager _dataFileObjectManager;

        private readonly ITencentOrderInfoManager _entityManager;
        private readonly IRepository<TencentOrderInfo, long> _entityRepository;

        /// <summary>
        ///     构造函数
        /// </summary>
        public TencentOrderInfoAppService(
            IRepository<TencentOrderInfo, long> entityRepository
            , ITencentOrderInfoManager entityManager,
            IRepository<BackgroundJobInfo, long> backgroundJobRepository, DataFileObjectManager dataFileObjectManager)
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
            _backgroundJobRepository = backgroundJobRepository;
            _dataFileObjectManager = dataFileObjectManager;
        }


        /// <summary>
        ///     获取TencentOrderInfo的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(TencentOrderInfoPermissions.Query)]
        public async Task<PagedResultDto<TencentOrderInfoListDto>> GetPaged(GetTencentOrderInfosInput input)
        {
            //  await   _backgroundJobManager.EnqueueAsync<TestJob, int>(42);


            var query = _entityRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件


            var count = await query.CountAsync();

            var entityList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var entityListDtos = ObjectMapper.Map<List<TencentOrderInfoListDto>>(entityList);
            //var entityListDtos = entityList.MapTo<List<TencentOrderInfoListDto>>();

            return new PagedResultDto<TencentOrderInfoListDto>(count, entityListDtos);
        }


        /// <summary>
        ///     通过指定id获取TencentOrderInfoListDto信息
        /// </summary>
        [AbpAuthorize(TencentOrderInfoPermissions.Query)]
        public async Task<TencentOrderInfoListDto> GetById(EntityDto<long> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return ObjectMapper.Map<TencentOrderInfoListDto>(entity);

            //return entity.MapTo<TencentOrderInfoListDto>();
        }

        /// <summary>
        ///     获取编辑 TencentOrderInfo
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(TencentOrderInfoPermissions.Create, TencentOrderInfoPermissions.Edit)]
        public async Task<GetTencentOrderInfoForEditOutput> GetForEdit(NullableIdDto<long> input)
        {
            var output = new GetTencentOrderInfoForEditOutput();
            TencentOrderInfoEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = ObjectMapper.Map<TencentOrderInfoEditDto>(entity);
                //editDto = entity.MapTo<TencentOrderInfoEditDto>();

                //tencentOrderInfoEditDto = ObjectMapper.Map<List<tencentOrderInfoEditDto>>(entity);
            }
            else
            {
                editDto = new TencentOrderInfoEditDto();
            }

            output.TencentOrderInfo = editDto;
            return output;
        }


        /// <summary>
        ///     添加或者修改TencentOrderInfo的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(TencentOrderInfoPermissions.Create, TencentOrderInfoPermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateTencentOrderInfoInput input)
        {
            if (input.TencentOrderInfo.Id.HasValue)
                await Update(input.TencentOrderInfo);
            else
                await Create(input.TencentOrderInfo);
        }


        /// <summary>
        ///     删除TencentOrderInfo信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(TencentOrderInfoPermissions.Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }


        /// <summary>
        ///     批量删除TencentOrderInfo的方法
        /// </summary>
        [AbpAuthorize(TencentOrderInfoPermissions.BatchDelete)]
        public async Task BatchDelete(List<long> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        public async Task DeleteSyncBackgroundJobs()
        {
            var backgroundJobInfos = await _backgroundJobRepository.GetAll()
                .Where(a => a.JobType.Contains("ImportTencentOrderToExcelJob")).ToListAsync();

            foreach (var info in backgroundJobInfos)
                if (info.IsAbandoned)
                {
                    var datafileArgs = JsonConvert.DeserializeObject<ImportTencentOrderToExcelJobArgs>(info.JobArgs);


                    await _backgroundJobRepository.DeleteAsync(info.Id);

                    await _dataFileObjectManager.DeleteAsync(datafileArgs.DataFileObjectId);
                }
        }

        /// <summary>
        ///     通过腾讯课程的order订单编码激活52abp的课程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SyncTencentOrderToL52ABPCourseOrder(EntityDto<long> input)
        {




            var tencentInfo = await _entityRepository.GetAll().FirstOrDefaultAsync(a => a.OrderNumber == input.Id);
            if (tencentInfo != null)
            {
                if (tencentInfo.TradingStatus == "支付成功")
                    await _entityManager.SyncTencentOrderToL52ABPCourseOrder(tencentInfo);
            }
            else
            {
                throw new UserFriendlyException($"您好,{AbpSession.GetUserName()},订单号:{input.Id}我们从系统中查询不到，请重试。");
            }
        }


        /// <summary>
        ///     新增TencentOrderInfo
        /// </summary>
        [AbpAuthorize(TencentOrderInfoPermissions.Create)]
        protected virtual async Task<TencentOrderInfoEditDto> Create(TencentOrderInfoEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<TencentOrderInfo>(input);
            //var entity = input.MapTo<TencentOrderInfo>();


            entity = await _entityRepository.InsertAsync(entity);
            //return entity.MapTo<TencentOrderInfoEditDto>();

            return ObjectMapper.Map<TencentOrderInfoEditDto>(entity);
        }

        /// <summary>
        ///     编辑TencentOrderInfo
        /// </summary>
        [AbpAuthorize(TencentOrderInfoPermissions.Edit)]
        protected virtual async Task Update(TencentOrderInfoEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            //input.MapTo(entity);

            ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }

        ///// <summary>
        ///// 导出TencentOrderInfo为excel表,等待开发。
        ///// </summary>
        ///// <returns></returns>
        //public async Task<FileDto> GetToExcel()
        //{
        //	var users = await UserManager.Users.ToListAsync();
        //	var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
        //	await FillRoleNames(userListDtos);
        //	return _userListExcelExporter.ExportToFile(userListDtos);
        //}
    }
}

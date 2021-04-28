using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using Abp.Threading;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.Excel.Importing.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.Excel.Importing
{
    /// <summary>
    /// 导入腾讯课程订单的job
    /// </summary>
    public class ImportTencentOrderToExcelJob : BackgroundJob<ImportTencentOrderToExcelJobArgs>, ITransientDependency
    {

        private readonly IDataFileObjectManager _dataFileObjectManager;
        private readonly ITencentOrderExcelDataReader _orderExcelDataReader;
        private readonly IObjectMapper _objectMapper;

        private readonly ITencentOrderInfoManager _tencentOrderInfoManager;

        public ImportTencentOrderToExcelJob(IDataFileObjectManager dataFileObjectManager, 
            ITencentOrderExcelDataReader orderExcelDataReader, IObjectMapper objectMapper,
            ITencentOrderInfoManager tencentOrderInfoManager)
        {
            _dataFileObjectManager = dataFileObjectManager;
            _orderExcelDataReader = orderExcelDataReader;
            _objectMapper = objectMapper;
            _tencentOrderInfoManager = tencentOrderInfoManager;
        }
 


        [UnitOfWork]

        public override void Execute(ImportTencentOrderToExcelJobArgs args)

        {

            var orders = GetTencentOrdersFromExcelOrNull(args);


            //创建到数据库中

            AsyncHelper.RunSync(()=> CreateTencentOrderInfo(orders));


        }

        private async Task CreateTencentOrderInfo(List<ImportTencentOrderDto> inputDtos)
        {


           

             var orderInfos=   _objectMapper.Map<List<TencentOrderInfo>>(inputDtos);

    await    _tencentOrderInfoManager.CreateTencentOrderInfo(orderInfos);

          


           
        }


        private List<ImportTencentOrderDto> GetTencentOrdersFromExcelOrNull(ImportTencentOrderToExcelJobArgs args)
        {
            try
            {

                var file = AsyncHelper.RunSync(() => _dataFileObjectManager.GetOrNullAsync(args.DataFileObjectId));

                return _orderExcelDataReader.GetTencentOrdersFromExcel(file.Bytes);
               
            }
            catch (Exception)
            {
                return null;

            }



        }





    }
}

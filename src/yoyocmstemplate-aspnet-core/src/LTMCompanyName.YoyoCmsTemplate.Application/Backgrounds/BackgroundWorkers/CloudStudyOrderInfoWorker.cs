using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Threading;
using Abp.Threading.Timers;
using L._52ABP.Common.Excel;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders.DomainService;

namespace LTMCompanyName.YoyoCmsTemplate.Backgrounds.BackgroundWorkers
{
    public class CloudStudyOrderInfoWorker : BackgroundJob<CloudStudyOrderInfoEventArgs>, ISingletonDependency
    {
        /// <summary>
        /// 循环执行的时间(60分钟)
        /// </summary>
        public const int WaitWorkMinutes = 60;

        /// <summary>
        /// 平台名称
        /// </summary>
        public const string platform = "网易云课堂";

        const string url_template = "http://cp.study.163.com/j/cp/downloadOrders.htm?providerId=400000000309007&tradeStartTime={0}&tradeEndTime={1}&tradeStatus={2}";

        private readonly INeteaseOrderInfoManager _neteaseOrderInfoManager;

        public readonly static DateTime startDate = new DateTime(1970, 1, 1, 8, 0, 0, 0);

        public CloudStudyOrderInfoWorker(
            AbpTimer timer,
            INeteaseOrderInfoManager neteaseOrderInfoManager
            )
        {
            _neteaseOrderInfoManager = neteaseOrderInfoManager;
        }


        public static void Execute()
        {
            IocManager.Instance.Resolve<CloudStudyOrderInfoWorker>().Execute(null);
        }

        [UnitOfWork]
        public override void Execute(CloudStudyOrderInfoEventArgs args)
        {
            AsyncHelper.RunSync(async () =>
            {

                args = new CloudStudyOrderInfoEventArgs();
                string requestUrl = null;


                var lastOrder = await _neteaseOrderInfoManager.GetLastOrder();
                if (lastOrder != null && lastOrder.OrderDate.HasValue)
                {
                    var tsStart = ConvertDateTime(lastOrder.OrderDate.Value, true) - startDate;
                    var tsEnd = ConvertDateTime(DateTime.Now) - startDate;

                    requestUrl = string.Format(url_template, Convert.ToInt64(tsStart.TotalMilliseconds).ToString(), Convert.ToInt64(tsEnd.TotalMilliseconds).ToString(), "-1");
                }


                var orderInfos = await CloudStudyOrderInfoWorker.ExecuteAsync(args, requestUrl);
                await _neteaseOrderInfoManager.Create(orderInfos, platform);
            });
        }

        public static async Task<List<NeteaseOrderInfo>> ExecuteAsync(CloudStudyOrderInfoEventArgs args, string requestUrl = null)
        {
            await Task.Yield();

            requestUrl = requestUrl ?? GetRequestUrl(args);

            var dataTable = GetDataTable(requestUrl);

            var orderInfos = ConvertToListOrderInfo(dataTable);

            return orderInfos;
        }

        private static string GetRequestUrl(CloudStudyOrderInfoEventArgs args)
        {
            /*
             -1 全部
             2 交易成功
             4 交易成功 发生退款
             5 交易成功 线下退款
             1 待支付
             3 交易关闭
             */
            var requestUrl = string.Empty;

            if (args.StartDate.HasValue && args.EndDate.HasValue)
            {
                var tsStart = ConvertDateTime(args.StartDate.Value, true) - startDate;
                var tsEnd = ConvertDateTime(args.EndDate.Value) - startDate;

                requestUrl = string.Format(url_template, Convert.ToInt64(tsStart.TotalMilliseconds).ToString(), Convert.ToInt64(tsEnd.TotalMilliseconds).ToString(), "-1");

                return requestUrl;
            }

            // 查询所有
            requestUrl = string.Format(url_template, "0", "0", "-1");

            return requestUrl;
        }


        private static DataTable GetDataTable(string requestUrl)
        {
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            var responseStream = response.GetResponseStream();

            var memoryStream = new MemoryStream();
            responseStream.CopyTo(memoryStream);


            var tmp = memoryStream.ExcelFileToDataTable(0);


            responseStream.Close();
            memoryStream.Close();


            return tmp;
        }



        private static List<NeteaseOrderInfo> ConvertToListOrderInfo(DataTable dataTable)
        {
            var orderInfoList = new List<NeteaseOrderInfo>();

            foreach (DataRow row in dataTable.Rows)
            {
                if (row[0] == null || row[0].ToString().IsNullOrWhiteSpace())
                {
                    continue;
                }

                var tmpObj = new NeteaseOrderInfo()
                {
                    OrderNo = row[0].ToString(),
                    OrderDate = DateTime.Parse(row[1].ToString()),
                    ProductType = row[2].ToString(),
                    ProductName = row[3].ToString(),
                    ProductDes = row[4]?.ToString(),
                    OriginalPrice = decimal.Parse(row[5].ToString()),
                    TransactionAmount = decimal.Parse(row[6].ToString()),
                    BuyerName = row[7].ToString(),
                    GeneralizeSource = row[8].ToString(),
                    GeneralizeName = row[9]?.ToString(),
                    CardAmount = 0m,
                    PlatformHB = 0m,
                    PracticalAmount = 0m,
                    PayType = row[13]?.ToString(),
                    ThirdPartyPayServiceCharge = 0m,
                    GeneralizeServiceCharge = 0m,
                    PlatformServiceCharge = 0m,
                    RealityAmount = 0m,
                    TransactionStatus = row[18].ToString(),
                    TransactionSuccessDate = null,
                    SettleAccounts = row[20].ToString(),
                    SettleAccountsDate = null,
                    Platform = platform
                };

                // 学习卡
                if (decimal.TryParse(row[10].ToString(), out decimal tmpDeciaml))
                {
                    tmpObj.CardAmount = tmpDeciaml;
                }

                // 平台红包
                if (decimal.TryParse(row[11].ToString(), out tmpDeciaml))
                {
                    tmpObj.PlatformHB = tmpDeciaml;
                }

                // 实际付款
                if (decimal.TryParse(row[12].ToString(), out tmpDeciaml))
                {
                    tmpObj.PracticalAmount = tmpDeciaml;
                }

                // 第三方支付费用
                if (decimal.TryParse(row[14].ToString(), out tmpDeciaml))
                {
                    tmpObj.ThirdPartyPayServiceCharge = tmpDeciaml;
                }

                // 渠道推广费用
                if (decimal.TryParse(row[15].ToString(), out tmpDeciaml))
                {
                    tmpObj.GeneralizeServiceCharge = tmpDeciaml;
                }

                // 平台服务费用
                if (decimal.TryParse(row[16].ToString(), out tmpDeciaml))
                {
                    tmpObj.PlatformServiceCharge = tmpDeciaml;
                }

                // 商家实际收入
                if (decimal.TryParse(row[17].ToString(), out tmpDeciaml))
                {
                    tmpObj.RealityAmount = tmpDeciaml;
                }

                // 交易成功时间
                if (DateTime.TryParse(row[19].ToString(), out DateTime dateTime))
                {
                    tmpObj.TransactionSuccessDate = dateTime;
                }

                // 结算时间
                if (DateTime.TryParse(row[20].ToString(), out dateTime))
                {
                    tmpObj.SettleAccountsDate = dateTime;
                }

                if (!orderInfoList.Exists(o => o.OrderNo == tmpObj.OrderNo))
                {
                    orderInfoList.Add(tmpObj);
                }

            }


            return orderInfoList;
        }

        private static DateTime ConvertDateTime(DateTime dateTime, bool isStart = false)
        {
            var dateStr = dateTime.ToString("yyyy-MM-dd");

            if (isStart)
            {
                return DateTime.Parse($"{dateStr} 00:00:00");
            }
            else
            {
                return DateTime.Parse($"{dateStr} 23:59:59");
            }

        }

    }

    public class CloudStudyOrderInfoEventArgs
    {

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        //public bool IsContinue { get; set; }
    }
}

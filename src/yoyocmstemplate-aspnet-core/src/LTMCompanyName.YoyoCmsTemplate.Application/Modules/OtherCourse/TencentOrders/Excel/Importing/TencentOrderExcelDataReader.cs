using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using LTMCompanyName.YoyoCmsTemplate.DataExporting.Excel.Epplus;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.Excel.Importing.Dto;
using OfficeOpenXml;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.Excel.Importing
{
    public class TencentOrderExcelDataReader : EpPlusExcelImporterBase<ImportTencentOrderDto>, ITencentOrderExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;

        public TencentOrderExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(AppConsts.LocalizationSourceName);
        }

        /// <summary>
        /// 从Excel中获取订单信息
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <returns></returns>
        public List<ImportTencentOrderDto> GetTencentOrdersFromExcel(byte[] fileBytes)
        {
         var dtos= ProcessExcelFile(fileBytes, ProcessExcelRow);

         return dtos;

        }

        /// <summary>
        /// 处理Excel中每行的信息
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private ImportTencentOrderDto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var order = new ImportTencentOrderDto();

            try
            {
                order.OrganizationId = Convert.ToInt32(worksheet.Cells[row, 1].Value);
                order.OrderNumber = Convert.ToInt32(worksheet.Cells[row, 2].Value);
                order.CourseTitle = worksheet.Cells[row, 3].Value.ToString();
                order.CourseType = worksheet.Cells[row, 4].Value.ToString();
                order.CreationTime = worksheet.Cells[row, 5].Value?.ToString();
                order.PurchaseTime = worksheet.Cells[row, 6].Value?.ToString();
                order.UIn = worksheet.Cells[row, 7].Value?.ToString();
                order.NickName = worksheet.Cells[row, 8].Value?.ToString();
                order.TradingStatus = worksheet.Cells[row, 9].Value?.ToString();
                order.MobileNumber = worksheet.Cells[row, 10].Value?.ToString();
                order.RecipientPhone = worksheet.Cells[row, 11].Value?.ToString();
                order.RecipientName = worksheet.Cells[row, 12].Value?.ToString();
                order.RecipientAddress = worksheet.Cells[row, 13].Value?.ToString();
                order.TradeType = worksheet.Cells[row, 14].Value?.ToString();
                order.UsersPay = Convert.ToDecimal(worksheet.Cells[row, 15].Value?.ToString());
                order.PlatformSubsidies = Convert.ToDecimal(worksheet.Cells[row, 16].Value?.ToString());
                order.RefundAmount = Convert.ToDecimal(worksheet.Cells[row, 17].Value?.ToString());
                order.AmountPaid = Convert.ToDecimal(worksheet.Cells[row, 18].Value?.ToString());
                order.SettlementAmount = Convert.ToDecimal(worksheet.Cells[row, 19].Value?.ToString());
                order.SettlementRatio = Convert.ToDecimal(worksheet.Cells[row, 20].Value?.ToString());
                order.ChannelFee = Convert.ToDecimal(worksheet.Cells[row, 21].Value?.ToString());
                order.TrafficConversionFee = Convert.ToDecimal(worksheet.Cells[row, 22].Value?.ToString());
                order.AppleShares = Convert.ToDecimal(worksheet.Cells[row, 23].Value?.ToString());
                order.DistributorShare = Convert.ToDecimal(worksheet.Cells[row, 24].Value?.ToString());
                order.SettlementStatus = worksheet.Cells[row, 25].Value?.ToString();
                order.IsIosOrder = GetStringConvertToBoolValueFromRow(worksheet, row, 26);
                order.IsDistributorOrder = GetStringConvertToBoolValueFromRow(worksheet, row, 27);
                order.OrderType = worksheet.Cells[row, 28].Value?.ToString();
                order.RemarkOne = worksheet.Cells[row, 29].Value?.ToString();
                order.RemarkTwo = worksheet.Cells[row, 30].Value?.ToString();
                order.RemarkThree = worksheet.Cells[row, 31].Value?.ToString();
            //    order.Password = GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(user.Password), exceptionMessage);
            //    order.AssignedRoleNames = GetAssignedRoleNamesFromRow(worksheet, row, 7);
          }
            catch (System.Exception exception)
            {
                order.Exception = exception.Message;
            }

            return order;
        }


        /// <summary>
        /// 根据是或者否返回bool值
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private bool GetStringConvertToBoolValueFromRow(ExcelWorksheet worksheet, int row, int column)
        {

            var cellValue = worksheet.Cells[row, column].Value;


            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
            {
                return false;
            }
            if (cellValue.ToString() == "是")
            {
                return true;

            }
            if (cellValue.ToString() == "否")
            {
                return false;
            }


            return false;


        }


 



    private string GetRequiredValueFromRowOrNull(ExcelWorksheet worksheet, int row, int column, string columnName, StringBuilder exceptionMessage)
        {
            var cellValue = worksheet.Cells[row, column].Value;

            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
            {
                return cellValue.ToString();
            }

            exceptionMessage.Append(GetLocalizedExceptionMessagePart(columnName));
            return null;
        }
        private string[] GetAssignedRoleNamesFromRow(ExcelWorksheet worksheet, int row, int column)
        {
            var cellValue = worksheet.Cells[row, column].Value;

            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
            {
                return new string[0];
            }

            return cellValue.ToString().Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToArray();
        }

        private string GetLocalizedExceptionMessagePart(string parameter)
        {
            return _localizationSource.GetString("{0}IsInvalid", _localizationSource.GetString(parameter)) + "; ";
        }

        private bool IsRowEmpty(ExcelWorksheet worksheet, int row)
        {
            return worksheet.Cells[row, 1].Value == null || string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Value.ToString());
        }



    }
}

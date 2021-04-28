using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Auditing.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Auditing.Dtos.EntityChange;
using LTMCompanyName.YoyoCmsTemplate.DataExporting.Excel.Epplus;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects.DataTempCache;

namespace LTMCompanyName.YoyoCmsTemplate.Auditing.Exporting
{
    [RemoteService(IsEnabled = false)]
    public class AuditLogListExcelExporter : EpplusExcelExporterBase, IAuditLogListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AuditLogListExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            IDataTempFileCacheManager dataTempFileCacheManager)
            : base(dataTempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportAuditLogToFile(List<AuditLogListDto> auditLogListDtos)
        {
            return CreateExcelPackage(
                "AuditLogs.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AuditLogs"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Time"),
                        L("UserName"),
                        L("Service"),
                        L("Action"),
                        L("Parameters"),
                        L("Duration"),
                        L("IpAddress"),
                        L("Client"),
                        L("Browser"),
                        L("ErrorState")
                    );

                    AddObject(
                        sheet, 2, auditLogListDtos,
                        _ => _timeZoneConverter.Convert(_.ExecutionTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.UserName,
                        _ => _.ServiceName,
                        _ => _.MethodName,
                        _ => _.Parameters,
                        _ => _.ExecutionDuration,
                        _ => _.ClientIpAddress,
                        _ => _.ClientName,
                        _ => _.BrowserInfo,
                        _ => _.Exception.IsNullOrEmpty() ? L("Success") : _.Exception
                        );

                    //Formatting cells

                    var timeColumn = sheet.Column(1);
                    timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    for (var i = 1; i <= 10; i++)
                    {
                        if (i.IsIn(5, 10)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }

        public FileDto ExportEntityChangeToFile(List<EntityChangeListDto> entityChangeListDtos)
        {
            return CreateExcelPackage(
                "DetailedLogs.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("DetailedLogs"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Action"),
                        L("Object"),
                        L("UserName"),
                        L("Time")
                    );

                    AddObject(
                        sheet, 2, entityChangeListDtos,
                        _ => _.ChangeType.ToString(),
                        _ => _.EntityTypeFullName,
                        _ => _.UserName,
                        _ => _timeZoneConverter.Convert(_.ChangeTime, _abpSession.TenantId, _abpSession.GetUserId())
                    );

                    //Formatting cells

                    var timeColumn = sheet.Column(1);
                    timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    for (var i = 1; i <= 10; i++)
                    {
                        if (i.IsIn(5, 10)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
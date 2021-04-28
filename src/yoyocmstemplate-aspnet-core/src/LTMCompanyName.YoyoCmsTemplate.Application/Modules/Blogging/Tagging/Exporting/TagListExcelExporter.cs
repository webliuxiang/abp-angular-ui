
using System;
using System.Collections.Generic;
using Abp.Application.Services;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos;
using LTMCompanyName.YoyoCmsTemplate.DataExporting.Excel.Epplus;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects.DataTempCache;


namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Exporting
{
    /// <summary>
    /// 标签的视图模型根据业务需要可以导出为Excel文件
    /// </summary>
	[RemoteService(IsEnabled = false)]
    public class TagListExcelExporter : EpplusExcelExporterBase, ITagListExcelExporter
    {
        /// <summary>
        /// 构造函数，需要继承父类
        /// </summary>
        /// <param name="dataTempFileCacheManager"></param>
        public TagListExcelExporter(IDataTempFileCacheManager dataTempFileCacheManager) : base(dataTempFileCacheManager)
        {

        }
        public FileDto ExportToExcelFile(List<TagListDto> tagListDtos)
        {

            var fileExportName = L("TagList") + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

            var excel =
                 CreateExcelPackage(fileExportName, excelpackage =>
               {
                   var sheet = excelpackage.Workbook.Worksheets.Add(L("Tags"));

                   sheet.OutLineApplyStyle = true;

                   AddHeader(sheet, L("TagName"), L("TagDescription"));
                   AddObject(sheet, 2, tagListDtos
                    , ex => ex.Name
                    , ex => ex.Description
                          );

                   //// custom codes



                   //// custom codes end
               });
            return excel;
        }
    }
}

using System;
using System.Collections.Generic;
using Abp.Application.Services;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs.Dtos;
using LTMCompanyName.YoyoCmsTemplate.DataExporting.Excel.Epplus;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects.DataTempCache;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs.Exporting
{
    /// <summary>
    /// 博客的视图模型根据业务需要可以导出为Excel文件
    /// </summary>
	[RemoteService(IsEnabled = false)]
    public class BlogListExcelExporter : EpplusExcelExporterBase, IBlogListExcelExporter
    {
        /// <summary>
        /// 构造函数，需要继承父类
        /// </summary>
        /// <param name="dataTempFileCacheManager"></param>
        public BlogListExcelExporter(IDataTempFileCacheManager dataTempFileCacheManager) : base(dataTempFileCacheManager)
        {
        }

        public FileDto ExportToExcelFile(List<BlogListDto> blogListDtos)
        {
            var fileExportName = L("BlogList") + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var excel =
               CreateExcelPackage(fileExportName, excelpackage =>
               {
                   var sheet = excelpackage.Workbook.Worksheets.Add(L("BlogList"));

                   sheet.OutLineApplyStyle = true;

                   AddHeader(sheet, L("BlogName"), L("BlogShortName"), L("BlogDescription"));
                   AddObject(sheet, 2, blogListDtos
                    , ex => ex.Name
                    , ex => ex.ShortName
                    , ex => ex.Description
                          );

                   //// custom codes

                   //// custom codes end
               });
            return excel;
        }
    }
}
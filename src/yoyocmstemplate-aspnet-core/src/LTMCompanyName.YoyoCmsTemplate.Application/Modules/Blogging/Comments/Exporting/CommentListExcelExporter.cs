
using System;
using System.Collections.Generic;
using Abp.Application.Services;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Comments.Dtos;
using LTMCompanyName.YoyoCmsTemplate.DataExporting.Excel.Epplus;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects.DataTempCache;


namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Comments.Exporting
{
    /// <summary>
    /// 评论的视图模型根据业务需要可以导出为Excel文件
    /// </summary>
	[RemoteService(IsEnabled = false)]
    public class CommentListExcelExporter : EpplusExcelExporterBase, ICommentListExcelExporter
    {
        /// <summary>
        /// 构造函数，需要继承父类
        /// </summary>
        /// <param name="dataTempFileCacheManager"></param>
        public CommentListExcelExporter(IDataTempFileCacheManager dataTempFileCacheManager) : base(dataTempFileCacheManager)
        {

        }
        public FileDto ExportToExcelFile(List<CommentListDto> commentListDtos)
        {

            var fileExportName = L("CommentList") + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

            var excel =
                 CreateExcelPackage(fileExportName, excelpackage =>
               {
                   var sheet = excelpackage.Workbook.Worksheets.Add(L("Comments"));

                   sheet.OutLineApplyStyle = true;

                   AddHeader(sheet, L("PostId"), L("RepliedCommentId"), L("CommentText"));
                   AddObject(sheet, 2, commentListDtos
                    , ex => ex.PostId
                    , ex => ex.RepliedCommentId
                    , ex => ex.Text
                          );

                   //// custom codes



                   //// custom codes end
               });
            return excel;
        }
    }
}

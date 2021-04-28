
using System;
using System.Collections.Generic;
using Abp.Application.Services;
using L._52ABP.Application.Dtos;
using L._52ABP.Common.Extensions.Enums;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Dtos;
using LTMCompanyName.YoyoCmsTemplate.DataExporting.Excel.Epplus;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects.DataTempCache;


namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Exporting
{
    /// <summary>
    /// 文章的视图模型根据业务需要可以导出为Excel文件
    /// </summary>
	[RemoteService(IsEnabled = false)]
    public class PostListExcelExporter : EpplusExcelExporterBase, IPostListExcelExporter
    {
        /// <summary>
        /// 构造函数，需要继承父类
        /// </summary>
        /// <param name="dataTempFileCacheManager"></param>
        public PostListExcelExporter(IDataTempFileCacheManager dataTempFileCacheManager) : base(dataTempFileCacheManager)
        {

        }
        public FileDto ExportToExcelFile(List<PostListDto> postListDtos)
        {

            var fileExportName = L("PostList") + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

            var excel =
                 CreateExcelPackage(fileExportName, excelpackage =>
               {
                   var sheet = excelpackage.Workbook.Worksheets.Add(L("Posts"));

                   sheet.OutLineApplyStyle = true;

                   AddHeader(sheet, L("BlogId"), L("PostUrl"), L("PostCoverImage"), L("PostTitle"), L("PostContent"), L("ReadCount"), L("PostType"), L("Tags"));
                   AddObject(sheet, 2, postListDtos
                    , ex => ex.BlogId
                    , ex => ex.Url
                    , ex => ex.CoverImage
                    , ex => ex.Title
                    , ex => ex.Content
                    , ex => ex.ReadCount
                   , ex => ex.PostType.ToDescription()
                    , ex => ex.Tags
                          );

                   //// custom codes



                   //// custom codes end
               });
            return excel;
        }
    }
}

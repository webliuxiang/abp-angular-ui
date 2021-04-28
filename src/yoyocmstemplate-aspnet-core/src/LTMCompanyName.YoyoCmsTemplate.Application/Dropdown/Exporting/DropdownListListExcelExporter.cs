
using System;
using System.Collections.Generic;
using Abp.Application.Services;
using L._52ABP.Application.Dtos;
using L._52ABP.Common.Extensions.Enums;

using LTMCompanyName.YoyoCmsTemplate.DataExporting.Excel.Epplus;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects.DataTempCache;
using LTMCompanyName.YoyoCmsTemplate.Dropdown.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Dropdown.Exporting;

namespace iNeu.Equipment.Dropdown.Exporting
{
    /// <summary>
    /// 下拉组件的视图模型根据业务需要可以导出为Excel文件
    /// </summary>
	[RemoteService(IsEnabled = false)]
    public class DropdownListListExcelExporter : EpplusExcelExporterBase, IDropdownListListExcelExporter
    {
        /// <summary>
        /// 构造函数，需要继承父类
        /// </summary>
        /// <param name="dataTempFileCacheManager"></param>
        public DropdownListListExcelExporter(IDataTempFileCacheManager dataTempFileCacheManager) : base(dataTempFileCacheManager)
        {

        }
        public FileDto ExportToExcelFile(List<DropdownListListDto> dropdownListListDtos)
        {

            var fileExportName = L("DropdownListList") + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

            var excel =
                 CreateExcelPackage(fileExportName, excelpackage =>
               {
                   var sheet = excelpackage.Workbook.Worksheets.Add(L("DropdownLists"));

                   sheet.OutLineApplyStyle = true;

                   AddHeader(sheet, L("DropdownListDDType_Id"), L("DropdownListName_TX"), L("IsActive_YN"), L("Parent"), L("DropdownListParentId"), L("DropdownListParentIdList"));
                   AddObject(sheet, 2, dropdownListListDtos
                    , ex => ex.DDType_Id
                    , ex => ex.Name_TX
                    , ex => ex.IsActive_YN
                   , ex => ex.Parent
                    , ex => ex.ParentId
                    , ex => ex.ParentIdList
                          );

                   //// custom codes



                   //// custom codes end
               });
            return excel;
        }
    }
}

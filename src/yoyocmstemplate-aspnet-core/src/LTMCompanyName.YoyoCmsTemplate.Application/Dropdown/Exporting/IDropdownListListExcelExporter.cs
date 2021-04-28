
using System.Collections.Generic;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Dropdown.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Dropdown.Exporting
{
    public interface IDropdownListListExcelExporter
    {
        /// <summary>
        /// 导出为Excel文件
        /// </summary>
        /// <param name="dropdownListListDtos">传入的下拉组件数据集合</param>
        /// <returns></returns>
        FileDto ExportToExcelFile(List<DropdownListListDto> dropdownListListDtos);

		
							//// custom codes
									
							

							//// custom codes end
    }
}

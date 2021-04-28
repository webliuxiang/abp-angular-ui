
using System.Collections.Generic;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Exporting
{
    public interface ITagListExcelExporter
    {
        /// <summary>
        /// 导出为Excel文件
        /// </summary>
        /// <param name="tagListDtos">传入的标签数据集合</param>
        /// <returns></returns>
        FileDto ExportToExcelFile(List<TagListDto> tagListDtos);


        //// custom codes



        //// custom codes end
    }
}
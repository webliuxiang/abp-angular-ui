
using System.Collections.Generic;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs.Exporting
{
    public interface IBlogListExcelExporter
    {
        /// <summary>
        /// 导出为Excel文件
        /// </summary>
        /// <param name="blogListDtos">传入的博客数据集合</param>
        /// <returns></returns>
        FileDto ExportToExcelFile(List<BlogListDto> blogListDtos);


        //// custom codes



        //// custom codes end
    }
}
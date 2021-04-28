
using System.Collections.Generic;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Exporting
{
    public interface IPostListExcelExporter
    {
        /// <summary>
        /// 导出为Excel文件
        /// </summary>
        /// <param name="postListDtos">传入的文章数据集合</param>
        /// <returns></returns>
        FileDto ExportToExcelFile(List<PostListDto> postListDtos);


        //// custom codes



        //// custom codes end
    }
}
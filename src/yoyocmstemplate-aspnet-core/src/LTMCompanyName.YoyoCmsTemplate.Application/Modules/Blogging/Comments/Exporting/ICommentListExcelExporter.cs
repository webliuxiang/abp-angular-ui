
using System.Collections.Generic;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Comments.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Comments.Exporting
{
    public interface ICommentListExcelExporter
    {
        /// <summary>
        /// 导出为Excel文件
        /// </summary>
        /// <param name="commentListDtos">传入的评论数据集合</param>
        /// <returns></returns>
        FileDto ExportToExcelFile(List<CommentListDto> commentListDtos);


        //// custom codes



        //// custom codes end
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Comments.Dtos;



namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Comments
{
    /// <summary>
    /// 评论应用层服务的接口方法
    ///</summary>
    public interface ICommentAppService : IApplicationService
    {
        /// <summary>
		/// 获取评论的分页列表集合
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<CommentListDto>> GetPaged(GetCommentsInput input);


        /// <summary>
        /// 通过指定id获取评论ListDto信息
        /// </summary>
        Task<CommentListDto> GetById(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体评论的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetCommentForEditOutput> GetForEdit(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改评论的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateCommentInput input);


        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除评论
        /// </summary>
        Task BatchDelete(List<Guid> input);




        /// <summary>
        /// 导出评论为excel文件
        /// </summary>
        /// <returns></returns>
        Task<FileDto> GetToExcelFile();




        //// custom codes



        //// custom codes end
    }
}


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Comments.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Comments.Exporting;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Comments;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Comments.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Blogging.Comments
{
    /// <summary>
    /// 评论应用层服务的接口实现方法
    ///</summary>
    [AbpAuthorize]
    public class CommentAppService : YoyoCmsTemplateAppServiceBase, ICommentAppService
    {
        private readonly IRepository<Comment, Guid> _commentRepository;

        private readonly IRepository<Post, Guid> _postRepository;

        private readonly ICommentListExcelExporter _commentListExcelExporter;

        private readonly ICommentManager _commentManager;
        /// <summary>
        /// 构造函数
        ///</summary>
        public CommentAppService(IRepository<Comment, Guid> commentRepository, ICommentManager commentManager, CommentListExcelExporter commentListExcelExporter, IRepository<Post, Guid> postRepository)
        {
            _commentRepository = commentRepository;
            _commentManager = commentManager;
            _commentListExcelExporter = commentListExcelExporter;
            _postRepository = postRepository;
        }


        /// <summary>
        /// 获取评论的分页列表信息
        ///      </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(CommentPermissions.Query)]
        public async Task<PagedResultDto<CommentListDto>> GetPaged(GetCommentsInput input)
        {

            var query = from entity in _commentRepository.GetAll()
                          .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a => a.Text.Contains(input.FilterText))
                        join post in _postRepository.GetAll() on entity.PostId equals post.Id into postJoind
                        from post in postJoind.DefaultIfEmpty()

                        join replied in _commentRepository.GetAll() on entity.RepliedCommentId equals replied.Id into repliedJoind
                        from replied in repliedJoind.DefaultIfEmpty()

                         select new CommentListDto()
                         {
                             Id = entity.Id,
                             PostId =entity.PostId,
                             RepliedCommentId =entity.RepliedCommentId,
                             Text =entity.Text,
                             Title =post.Title,
                             RepliedCommentTest = replied!=null ? replied.Text : ""
                         };


            var count = await query.CountAsync();

            var commentList = await query
                    .PageBy(input)
                    .ToListAsync();

            return new PagedResultDto<CommentListDto>(count, commentList);
        }


        /// <summary>
        /// 通过指定id获取CommentListDto信息
        /// </summary>
        [AbpAuthorize(CommentPermissions.Query)]
        public async Task<CommentListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _commentRepository.GetAsync(input.Id);

            var dto = ObjectMapper.Map<CommentListDto>(entity);
            return dto;
        }

        /// <summary>
        /// 获取编辑 评论
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(CommentPermissions.Create, CommentPermissions.Edit)]
        public async Task<GetCommentForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetCommentForEditOutput();
            CommentEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _commentRepository.GetAsync(input.Id.Value);
                editDto = ObjectMapper.Map<CommentEditDto>(entity);
            }
            else
            {
                editDto = new CommentEditDto();
            }



            output.Comment = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改评论的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(CommentPermissions.Create, CommentPermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateCommentInput input)
        {

            if (input.Comment.Id.HasValue)
            {
                await Update(input.Comment);
            }
            else
            {
                await Create(input.Comment);
            }
        }


        /// <summary>
        /// 新增评论
        /// </summary>
        [AbpAuthorize(CommentPermissions.Create)]
        protected virtual async Task<CommentEditDto> Create(CommentEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<Comment>(input);
            entity.Id = SequentialGuidGenerator.Instance.Create();

            //调用领域服务
            entity = await _commentManager.CreateAsync(entity);

            var dto = ObjectMapper.Map<CommentEditDto>(entity);
            return dto;
        }

        /// <summary>
        /// 编辑评论
        /// </summary>
        [AbpAuthorize(CommentPermissions.Edit)]
        protected virtual async Task Update(CommentEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _commentRepository.GetAsync(input.Id.Value);
            //  input.MapTo(entity);
            //将input属性的值赋值到entity中
            ObjectMapper.Map(input, entity);
            await _commentManager.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除评论信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(CommentPermissions.Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _commentManager.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Comment的方法
        /// </summary>
        [AbpAuthorize(CommentPermissions.BatchDelete)]
        public async Task BatchDelete(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _commentManager.BatchDelete(input);
        }




        /// <summary>
        /// 导出评论为excel文件
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(CommentPermissions.ExportExcel)]
        public async Task<FileDto> GetToExcelFile()
        {
            var comments = await _commentManager.QueryComments().ToListAsync();
            var commentListDtos = ObjectMapper.Map<List<CommentListDto>>(comments);
            return _commentListExcelExporter.ExportToExcelFile(commentListDtos);
        }



        //// custom codes



        //// custom codes end

    }
}



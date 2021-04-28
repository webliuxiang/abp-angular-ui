
using AutoMapper;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Comments;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Comments.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Blogs;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Comments;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Posts.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Blogging.Tagging;

namespace LTMCompanyName.YoyoCmsTemplate.CustomDtoAutoMapper
{

    /// <summary>
    /// 配置Blog的AutoMapper映射
    /// 前往 <see cref="YoyoCmsTemplateApplicationModule"/>的AbpAutoMapper配置方法下添加以下代码段
    /// BlogDtoAutoMapper.CreateMappings(configuration);
    /// </summary>
    internal static class BlogDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Blog, BlogListDto>()
             .ForMember(x => x.BlogUserName, o => o.MapFrom(s => s.BlogUser.UserName));

            configuration.CreateMap<BlogEditDto, Blog>();
            configuration.CreateMap<Blog, BlogEditDto>()
                .ForMember(x=>x.BlogUserId,o=>o.MapFrom(s=>s.BlogUser.Id));


            configuration.CreateMap<Comment, CommentListDto>();
            configuration.CreateMap<CommentListDto, Comment>();

            configuration.CreateMap<CommentEditDto, Comment>();
            configuration.CreateMap<Comment, CommentEditDto>();

            configuration.CreateMap<Tag, TagListDto>();
            configuration.CreateMap<TagListDto, Tag>();

            configuration.CreateMap<TagEditDto, Tag>();
            configuration.CreateMap<Tag, TagEditDto>();

            configuration.CreateMap<Post, PostListDto>();
            configuration.CreateMap<PostListDto, Post>();

            //configuration.CreateMap<PostEditDto, Post>().ForMember(dto => dto.Tags, options => options.Ignore()); ;
            configuration.CreateMap<Post, CreatePostDto>();
            configuration.CreateMap<CreatePostDto, Post>();
            configuration.CreateMap<PostEditDto, Post>();
            configuration.CreateMap<Post, PostEditDto>();
        
            configuration.CreateMap<Post, PostDetailsDto>();

        }
    }
}

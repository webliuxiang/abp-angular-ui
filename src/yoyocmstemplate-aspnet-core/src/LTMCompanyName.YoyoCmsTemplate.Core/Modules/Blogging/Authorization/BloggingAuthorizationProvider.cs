

using System.Linq;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;

// ReSharper disable once CheckNamespace
namespace LTMCompanyName.YoyoCmsTemplate.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="BlogPermissions" /> for all permission names. Blog
    ///</summary>
    public class BloggingAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public BloggingAuthorizationProvider()
        {

        }


        public BloggingAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public BloggingAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            // 在这里配置了Blog 的权限。
            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));


            //// custom codes

            var blogModule = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_BlogModule) ?? pages.CreateChildPermission(AppPermissions.Pages_BlogModule, L("BlogModule"));

            var blogs = blogModule.CreateChildPermission(BlogPermissions.Node, L("Blog"));
            blogs.CreateChildPermission(BlogPermissions.Query, L("QueryBlog"));
            blogs.CreateChildPermission(BlogPermissions.Create, L("CreateBlog"));
            blogs.CreateChildPermission(BlogPermissions.Edit, L("EditBlog"));
            blogs.CreateChildPermission(BlogPermissions.Delete, L("DeleteBlog"));
            blogs.CreateChildPermission(BlogPermissions.BatchDelete, L("BatchDeleteBlog"));
            blogs.CreateChildPermission(BlogPermissions.ExportExcel, L("ExportToExcel"));
            blogs.CreateChildPermission(BlogPermissions.BlogSelectUser, L("BlogSelectUser"));

            var posts = blogModule.CreateChildPermission(PostPermissions.Node, L("Post"));
            posts.CreateChildPermission(PostPermissions.Query, L("QueryPost"));
            posts.CreateChildPermission(PostPermissions.Create, L("CreatePost"));
            posts.CreateChildPermission(PostPermissions.Edit, L("EditPost"));
            posts.CreateChildPermission(PostPermissions.Delete, L("DeletePost"));
            posts.CreateChildPermission(PostPermissions.BatchDelete, L("BatchDeletePost"));
            posts.CreateChildPermission(PostPermissions.ExportExcel, L("ExportToExcel"));
            var comments = blogModule.CreateChildPermission(CommentPermissions.Node, L("Comment"));
            comments.CreateChildPermission(CommentPermissions.Query, L("QueryComment"));
            comments.CreateChildPermission(CommentPermissions.Create, L("CreateComment"));
            comments.CreateChildPermission(CommentPermissions.Edit, L("EditComment"));
            comments.CreateChildPermission(CommentPermissions.Delete, L("DeleteComment"));
            comments.CreateChildPermission(CommentPermissions.BatchDelete, L("BatchDeleteComment"));
            comments.CreateChildPermission(CommentPermissions.ExportExcel, L("ExportToExcel"));

            var tags = blogModule.CreateChildPermission(TagPermissions.Node, L("Tag"));
            tags.CreateChildPermission(TagPermissions.Query, L("QueryTag"));
            tags.CreateChildPermission(TagPermissions.Create, L("CreateTag"));
            tags.CreateChildPermission(TagPermissions.Edit, L("EditTag"));
            tags.CreateChildPermission(TagPermissions.Delete, L("DeleteTag"));
            tags.CreateChildPermission(TagPermissions.BatchDelete, L("BatchDeleteTag"));
            tags.CreateChildPermission(TagPermissions.ExportExcel, L("ExportToExcel"));
            //// custom codes end
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AppConsts.LocalizationSourceName);
        }
    }
}


using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Blogs.Dtos;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace LTMCompanyName.YoyoCmsTemplate.Tests.Blogs
{
    public class BlogAppService_Tests : YoyoCmsTemplateTestBase
    {
        private readonly IBlogAppService _blogAppService;

        public BlogAppService_Tests()
        {
            _blogAppService = Resolve<IBlogAppService>();
        }

        [Fact]
        public async Task CreateBlog_Test()
        {
            await _blogAppService.CreateOrUpdate(new CreateOrUpdateBlogInput
            {
                Blog = new BlogEditDto
                {


                    Name = "test",
                    ShortName = "test",
                    Description = "test",


                }
            });

            await UsingDbContextAsync(async context =>
            {
                var dystopiaBlog = await context.Blogs.FirstOrDefaultAsync();
                dystopiaBlog.ShouldNotBeNull();
            }
            );
        }

        [Fact]
        public async Task GetBlogs_Test()
        {
            // Act
            var output = await _blogAppService.GetPaged(new GetBlogsInput
            {
                MaxResultCount = 20,
                SkipCount = 0
            });

            // Assert
            output.Items.Count.ShouldBeGreaterThanOrEqualTo(0);
        }


        //// custom codes



        //// custom codes end


    }
}
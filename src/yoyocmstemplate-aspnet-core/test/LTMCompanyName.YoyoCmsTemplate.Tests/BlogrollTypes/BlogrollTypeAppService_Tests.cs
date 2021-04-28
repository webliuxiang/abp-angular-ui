
using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Blogrolls.Dtos;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace LTMCompanyName.YoyoCmsTemplate.Tests.BlogrollTypes
{
    public class BlogrollTypeAppService_Tests : YoyoCmsTemplateTestBase
    {
        private readonly IBlogrollTypeAppService _blogrollTypeAppService;

        public BlogrollTypeAppService_Tests()
        {
            _blogrollTypeAppService = Resolve<IBlogrollTypeAppService>();
        }

        [Fact]
        public async Task CreateBlogrollType_Test()
        {
            await _blogrollTypeAppService.CreateOrUpdate(new CreateOrUpdateBlogrollTypeInput
            {
                BlogrollType = new BlogrollTypeEditDto
                {


                    Name = "test",


                }
            });

            await UsingDbContextAsync(async context =>
            {
                var dystopiaBlogrollType = await context.BlogrollTypes.FirstOrDefaultAsync();
                dystopiaBlogrollType.ShouldNotBeNull();
            }
            );
        }

        [Fact]
        public async Task GetBlogrollTypes_Test()
        {
            // Act
            var output = await _blogrollTypeAppService.GetPaged(new GetBlogrollTypesInput
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

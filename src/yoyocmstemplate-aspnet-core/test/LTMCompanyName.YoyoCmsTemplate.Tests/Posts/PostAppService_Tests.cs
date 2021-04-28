
using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Posts.Dtos;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace LTMCompanyName.YoyoCmsTemplate.Tests.Posts
{
    public class PostAppService_Tests : YoyoCmsTemplateTestBase
    {
        private readonly IPostAppService _postAppService;

        public PostAppService_Tests()
        {
            _postAppService = Resolve<IPostAppService>();
        }

        [Fact]
        public async Task CreatePost_Test()
        {
            await _postAppService.CreateOrUpdate(new CreateOrUpdatePostInput
            {
                Post = new PostEditDto
                {


                    Url = "test",
                    CoverImage = "test",
                    Title = "test",
                    Content = "test",
                    // 枚举类型需要自己添加完善


                }
            });

            await UsingDbContextAsync(async context =>
            {
                var dystopiaPost = await context.Posts.FirstOrDefaultAsync();
                dystopiaPost.ShouldNotBeNull();
            }
            );
        }

        [Fact]
        public async Task GetPosts_Test()
        {
            // Act
            var output = await _postAppService.GetPaged(new GetPostsInput
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
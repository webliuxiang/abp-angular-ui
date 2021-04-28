
using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Comments;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Comments.Dtos;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace LTMCompanyName.YoyoCmsTemplate.Tests.Comments
{
    public class CommentAppService_Tests : YoyoCmsTemplateTestBase
    {
        private readonly ICommentAppService _commentAppService;

        public CommentAppService_Tests()
        {
            _commentAppService = Resolve<ICommentAppService>();
        }

        [Fact]
        public async Task CreateComment_Test()
        {
            await _commentAppService.CreateOrUpdate(new CreateOrUpdateCommentInput
            {
                Comment = new CommentEditDto
                {


                    Text = "test",


                }
            });

            await UsingDbContextAsync(async context =>
            {
                var dystopiaComment = await context.Comments.FirstOrDefaultAsync();
                dystopiaComment.ShouldNotBeNull();
            }
            );
        }

        [Fact]
        public async Task GetComments_Test()
        {
            // Act
            var output = await _commentAppService.GetPaged(new GetCommentsInput
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
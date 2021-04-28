
using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging;
using LTMCompanyName.YoyoCmsTemplate.Blogging.Tagging.Dtos;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace LTMCompanyName.YoyoCmsTemplate.Tests.Tags
{
    public class TagAppService_Tests : YoyoCmsTemplateTestBase
    {
        private readonly ITagAppService _tagAppService;

        public TagAppService_Tests()
        {
            _tagAppService = Resolve<ITagAppService>();
        }

        [Fact]
        public async Task CreateTag_Test()
        {
            await _tagAppService.CreateOrUpdate(new CreateOrUpdateTagInput
            {
                Tag = new TagEditDto
                {


                    Name = "test",
                    Description = "test",


                }
            });

            await UsingDbContextAsync(async context =>
            {
                var dystopiaTag = await context.Tags.FirstOrDefaultAsync();
                dystopiaTag.ShouldNotBeNull();
            }
            );
        }

        [Fact]
        public async Task GetTags_Test()
        {
            // Act
            var output = await _tagAppService.GetPaged(new GetTagsInput
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
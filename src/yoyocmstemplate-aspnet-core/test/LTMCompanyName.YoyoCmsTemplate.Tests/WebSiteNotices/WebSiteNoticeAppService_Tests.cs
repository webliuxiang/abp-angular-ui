
using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Notices;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.Notices.Dtos;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace LTMCompanyName.YoyoCmsTemplate.Tests.WebSiteNotices
{
    public class WebSiteNoticeAppService_Tests : YoyoCmsTemplateTestBase
    {
        private readonly IWebSiteNoticeAppService _webSiteNoticeAppService;

        public WebSiteNoticeAppService_Tests()
        {
            _webSiteNoticeAppService = Resolve<IWebSiteNoticeAppService>();
        }

        [Fact]
        public async Task CreateWebSiteNotice_Test()
        {
            await _webSiteNoticeAppService.CreateOrUpdate(new CreateOrUpdateWebSiteNoticeInput
            {
                WebSiteNotice = new WebSiteNoticeEditDto
                {


                    Title = "test",
                    Content = "test",


                }
            });

            await UsingDbContextAsync(async context =>
            {
                var dystopiaWebSiteNotice = await context.WebSiteNotices.FirstOrDefaultAsync();
                dystopiaWebSiteNotice.ShouldNotBeNull();
            }
            );
        }

        [Fact]
        public async Task GetWebSiteNotices_Test()
        {
            // Act
            var output = await _webSiteNoticeAppService.GetPaged(new GetWebSiteNoticesInput
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
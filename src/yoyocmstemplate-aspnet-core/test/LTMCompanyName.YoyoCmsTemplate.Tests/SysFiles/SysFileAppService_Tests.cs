using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.SystemBaseManage;
using LTMCompanyName.YoyoCmsTemplate.SystemBaseManage.Dtos;
using Shouldly;
using Xunit;

namespace LTMCompanyName.YoyoCmsTemplate.Tests.SysFiles
{
    public class SysFileAppService_Tests : YoyoCmsTemplateTestBase
    {
        private readonly ISysFileAppService _sysFileAppService;

        public SysFileAppService_Tests()
        {
            _sysFileAppService = Resolve<ISysFileAppService>();
        }

        [Fact]
        public async Task GetSysFiles_Test()
        {
            // Act
            var output = await _sysFileAppService.GetPaged(new GetSysFilesInput
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

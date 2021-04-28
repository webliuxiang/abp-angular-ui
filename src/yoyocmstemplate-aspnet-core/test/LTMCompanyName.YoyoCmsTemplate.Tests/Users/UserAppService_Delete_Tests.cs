using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Shouldly;
using Xunit;

namespace LTMCompanyName.YoyoCmsTemplate.Tests.Users
{
    // ReSharper disable once InconsistentNaming
    public class UserAppService_Delete_Tests : UserAppServiceTestBase
    {
        [Fact]
        public async Task Should_Delete_User()
        {
            //Arrange
            CreateTestUsers();

            var user = await GetUserByUserNameOrNullAsync("artdent");
            user.ShouldNotBe(null);

            //Act
            await UserAppService.Delete(new EntityDto<long>(user.Id));

            //Assert
            user = await GetUserByUserNameOrNullAsync("artdent");
            user.IsDeleted.ShouldBe(true);
        }
    }
}
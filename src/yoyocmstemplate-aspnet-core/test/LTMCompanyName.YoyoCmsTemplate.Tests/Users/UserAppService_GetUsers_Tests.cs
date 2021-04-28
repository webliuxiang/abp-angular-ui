using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos;
using Shouldly;
using Xunit;

namespace LTMCompanyName.YoyoCmsTemplate.Tests.Users
{
    // ReSharper disable once InconsistentNaming
    public class UserAppService_GetUsers_Tests : UserAppServiceTestBase
    {

        [Fact]
        public async Task Should_Get_Initial_Users()
        {
            //Act
            var output = await UserAppService.GetPaged(new GetUsersInput());

            //Assert
            output.TotalCount.ShouldBe(1);
            output.Items.Count.ShouldBe(1);
            output.Items[0].UserName.ShouldBe(AbpUserBase.AdminUserName);
        }

        [Fact]
        public async Task Should_Get_Users_Paged_And_Sorted_And_Filtered()
        {
            //Arrange
            CreateTestUsers();

            //Act
            var output = await UserAppService.GetPaged(
                new GetUsersInput
                {
                    MaxResultCount = 2,
                    Sorting = "Username"
                });

            //Assert
            output.TotalCount.ShouldBe(4);
            output.Items.Count.ShouldBe(2);
            output.Items[0].UserName.ShouldBe("adams_d");
            output.Items[1].UserName.ShouldBe(AbpUserBase.AdminUserName);
        }

        [Fact]
        public async Task Should_Get_Users_Filtered()
        {
            //Arrange
            CreateTestUsers();

            //Act
            var output = await UserAppService.GetPaged(
                new GetUsersInput());

            //Assert
            output.TotalCount.ShouldBe(4);
            output.Items.Count.ShouldBe(4);
            output.Items[0].UserName.ShouldBe("admin");

            //Act
            var output2 = await UserAppService.GetPaged(
                new GetUsersInput
                {
                    Role = new List<int>() { 1 }
                });

            //Assert
            output2.TotalCount.ShouldBe(0);
            output2.Items.Count.ShouldBe(0);
        }
    }
}

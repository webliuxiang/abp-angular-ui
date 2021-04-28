using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace LTMCompanyName.YoyoCmsTemplate.Tests.Users
{
    public class UserAppService_Tests : YoyoCmsTemplateTestBase
    {
        public UserAppService_Tests()
        {
            _userAppService = Resolve<IUserAppService>();
        }

        private readonly IUserAppService _userAppService;

        [Fact]
        public async Task CreateUser_Test()
        {
            // Act
            await _userAppService.CreateOrUpdate(new CreateOrUpdateUserInput
            {
                User = new UserEditDto
                {
                    EmailAddress = "test@live.com.com",
                    IsActive = true,
                    Password = User.DefaultPassword,
                    UserName = "test"
                },
                AssignedRoleNames = new string[0]
            });


            await UsingDbContextAsync(async context =>
            {
                var johnNashUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "test");
                johnNashUser.ShouldNotBeNull();
            });
        }

        [Fact]
        public async Task GetUsers_Test()
        {
            // Act
            var output = await _userAppService.GetPaged(new GetUsersInput
            {
                MaxResultCount = 20,
                SkipCount = 0
            });

            // Assert
            output.Items.Count.ShouldBeGreaterThan(0);
        }
    }
}
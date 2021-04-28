using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Profile;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Profile.Dtos;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Xunit;

namespace LTMCompanyName.YoyoCmsTemplate.Tests.Users
{
    // ReSharper disable once InconsistentNaming
    public class ProfileAppService_Tests : YoyoCmsTemplateTestBase
    {
        private readonly IProfileAppService _profileAppService;

        public ProfileAppService_Tests()
        {
            _profileAppService = Resolve<IProfileAppService>();
        }

        [Fact]
        public async Task GetUserProfileForEdit_Test()
        {
            //Act
            var output = await _profileAppService.GetCurrentUserProfileForEdit();

            //Assert
            var currentUser = await GetCurrentUserAsync();
            //output.Name.ShouldBe(currentUser.Name);
            //output.Surname.ShouldBe(currentUser.Surname);
            output.EmailAddress.ShouldBe(currentUser.EmailAddress);
        }

        [Fact]
        public async Task UpdateUserProfileForEdit_Test()
        {
            //Arrange
            var currentUser = await GetCurrentUserAsync();

            //Act
            await _profileAppService.UpdateCurrentUserProfile(
                new CurrentUserProfileEditDto
                {
                    UserName = currentUser.UserName,
                    FullName = "modified name",
                    EmailAddress = "test1@test.net",
                    PhoneNumber = "13388888888",
                    IsPhoneNumberConfirmed = true
                });

            //Assert
            currentUser = await GetCurrentUserAsync();
            currentUser.EmailAddress.ShouldBe("test1@test.net");
            currentUser.PhoneNumber.ShouldBe("13388888888");
        }

        [Fact]
        public async Task ChangePassword_Test()
        {
            //Act
            await _profileAppService.ChangePassword(
                new ChangePasswordInput
                {
                    CurrentPassword = "bb123456",
                    NewPassword = "2mF9d8Ac!5"
                });

            //Assert
            var currentUser = await GetCurrentUserAsync();

            LocalIocManager
                .Resolve<IPasswordHasher<User>>()
                .VerifyHashedPassword(currentUser, currentUser.Password, "2mF9d8Ac!5")
                .ShouldBe(PasswordVerificationResult.Success);
        }
    }
}

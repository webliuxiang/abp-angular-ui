using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles.Dtos;
using Shouldly;
using Xunit;

namespace LTMCompanyName.YoyoCmsTemplate.Tests.Authorization.Roles
{
    // ReSharper disable once InconsistentNaming
    public class RoleAppService_Tests : YoyoCmsTemplateTestBase
    {
        private readonly IRoleAppService _roleAppService;

        public RoleAppService_Tests()
        {
            _roleAppService = Resolve<IRoleAppService>();
        }

        [MultiTenantFact]
        public async Task Should_GetAll_Roles_For_Host()
        {
            LoginAsHostAdmin();

            //Act
            var output = await _roleAppService.GetAll(new GetRolesInput());

            //Assert
            output.Items.Count.ShouldBe(1);
        }

        [Fact]
        public async Task Should_Get_Roles_For_Tenant()
        {
            //Act
            var output = await _roleAppService.GetPaged(new GetRolePagedInput());

            //Assert
            output.Items.Count.ShouldBe(1);
        }
    }
}

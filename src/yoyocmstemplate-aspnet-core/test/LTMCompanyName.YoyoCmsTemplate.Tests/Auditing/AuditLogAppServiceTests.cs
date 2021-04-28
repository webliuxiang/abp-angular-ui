using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Timing;
using LTMCompanyName.YoyoCmsTemplate.Auditing;
using LTMCompanyName.YoyoCmsTemplate.Auditing.Dtos;
using Shouldly;
using Xunit;

namespace LTMCompanyName.YoyoCmsTemplate.Tests.Auditing
{
    public class AuditLogAppServiceTests : YoyoCmsTemplateTestBase
    {
        private readonly IAuditLogAppService _auditLogAppService;

        public AuditLogAppServiceTests()
        {
            _auditLogAppService = Resolve<IAuditLogAppService>();
        }

        [Fact(Skip = "Todo sorting")]
        public async Task Should_Get_Audit_Logs()
        {
            //Arrange
            UsingDbContext(
                context =>
                {
                    context.AuditLogs.Add(
                        new AuditLog
                        {
                            TenantId = AbpSession.TenantId,
                            UserId = AbpSession.UserId,
                            ServiceName = "ServiceName-Test-1",
                            MethodName = "MethodName-Test-1",
                            Parameters = "{}",
                            ExecutionTime = Clock.Now.AddMinutes(-1),
                            ExecutionDuration = 123
                        });

                    context.AuditLogs.Add(
                        new AuditLog
                        {
                            TenantId = AbpSession.TenantId,
                            ServiceName = "ServiceName-Test-2",
                            MethodName = "MethodName-Test-2",
                            Parameters = "{}",
                            ExecutionTime = Clock.Now,
                            ExecutionDuration = 456
                        });
                });

            //Act
            var output = await _auditLogAppService.GetPagedAuditLogs(new GetAuditLogsInput
            {
                StartDate = Clock.Now.AddMinutes(-10),
                EndDate = Clock.Now.AddMinutes(10),
                Sorting = "ExecutionTime Desc"
            });

            output.TotalCount.ShouldBe(2);

            output.Items[0].ServiceName.ShouldBe("ServiceName-Test-2");
            output.Items[0].UserName.ShouldBe(null);

            output.Items[1].ServiceName.ShouldBe("ServiceName-Test-1");

            output.Items[1].UserName.ShouldBe(AbpUserBase.AdminUserName, StringCompareShould.IgnoreCase);
        }
    }
}
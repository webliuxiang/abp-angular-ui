using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LTMCompanyName.YoyoCmsTemplate.Migrations
{
    public partial class Initial_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpAuditLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    ServiceName = table.Column<string>(maxLength: 256, nullable: true),
                    MethodName = table.Column<string>(maxLength: 256, nullable: true),
                    Parameters = table.Column<string>(maxLength: 1024, nullable: true),
                    ReturnValue = table.Column<string>(nullable: true),
                    ExecutionTime = table.Column<DateTime>(nullable: false),
                    ExecutionDuration = table.Column<int>(nullable: false),
                    ClientIpAddress = table.Column<string>(maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(maxLength: 128, nullable: true),
                    BrowserInfo = table.Column<string>(maxLength: 512, nullable: true),
                    Exception = table.Column<string>(maxLength: 2000, nullable: true),
                    ImpersonatorUserId = table.Column<long>(nullable: true),
                    ImpersonatorTenantId = table.Column<int>(nullable: true),
                    CustomData = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpBackgroundJobs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    JobType = table.Column<string>(maxLength: 512, nullable: false),
                    JobArgs = table.Column<string>(maxLength: 1048576, nullable: false),
                    TryCount = table.Column<short>(nullable: false),
                    NextTryTime = table.Column<DateTime>(nullable: false),
                    LastTryTime = table.Column<DateTime>(nullable: true),
                    IsAbandoned = table.Column<bool>(nullable: false),
                    Priority = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpBackgroundJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpDynamicProperties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyName = table.Column<string>(nullable: true),
                    InputType = table.Column<string>(nullable: true),
                    Permission = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpDynamicProperties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpEditions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 64, nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    ExpiringEditionId = table.Column<int>(nullable: true),
                    MonthlyPrice = table.Column<decimal>(nullable: true),
                    AnnualPrice = table.Column<decimal>(nullable: true),
                    TrialDayCount = table.Column<int>(nullable: true),
                    WaitingDayAfterExpire = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityChangeSets",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrowserInfo = table.Column<string>(maxLength: 512, nullable: true),
                    ClientIpAddress = table.Column<string>(maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(maxLength: 128, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    ExtensionData = table.Column<string>(nullable: true),
                    ImpersonatorTenantId = table.Column<int>(nullable: true),
                    ImpersonatorUserId = table.Column<long>(nullable: true),
                    Reason = table.Column<string>(maxLength: 256, nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityChangeSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 64, nullable: false),
                    Icon = table.Column<string>(maxLength: 128, nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpLanguages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpLanguageTexts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    LanguageName = table.Column<string>(maxLength: 128, nullable: false),
                    Source = table.Column<string>(maxLength: 128, nullable: false),
                    Key = table.Column<string>(maxLength: 256, nullable: false),
                    Value = table.Column<string>(maxLength: 67108864, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpLanguageTexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    NotificationName = table.Column<string>(maxLength: 96, nullable: false),
                    Data = table.Column<string>(maxLength: 1048576, nullable: true),
                    DataTypeName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityTypeName = table.Column<string>(maxLength: 250, nullable: true),
                    EntityTypeAssemblyQualifiedName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityId = table.Column<string>(maxLength: 96, nullable: true),
                    Severity = table.Column<byte>(nullable: false),
                    UserIds = table.Column<string>(maxLength: 131072, nullable: true),
                    ExcludedUserIds = table.Column<string>(maxLength: 131072, nullable: true),
                    TenantIds = table.Column<string>(maxLength: 131072, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpNotificationSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    NotificationName = table.Column<string>(maxLength: 96, nullable: true),
                    EntityTypeName = table.Column<string>(maxLength: 250, nullable: true),
                    EntityTypeAssemblyQualifiedName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityId = table.Column<string>(maxLength: 96, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpNotificationSubscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpOrganizationUnitRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    OrganizationUnitId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpOrganizationUnitRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpOrganizationUnits",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    ParentId = table.Column<long>(nullable: true),
                    Code = table.Column<string>(maxLength: 95, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpOrganizationUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpOrganizationUnits_AbpOrganizationUnits_ParentId",
                        column: x => x.ParentId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpPersistedGrants",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 200, nullable: false),
                    Type = table.Column<string>(maxLength: 50, nullable: false),
                    SubjectId = table.Column<string>(maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: true),
                    Data = table.Column<string>(maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpPersistedGrants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpTenantNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    NotificationName = table.Column<string>(maxLength: 96, nullable: false),
                    Data = table.Column<string>(maxLength: 1048576, nullable: true),
                    DataTypeName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityTypeName = table.Column<string>(maxLength: 250, nullable: true),
                    EntityTypeAssemblyQualifiedName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityId = table.Column<string>(maxLength: 96, nullable: true),
                    Severity = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpTenantNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    UserLinkId = table.Column<long>(nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    EmailAddress = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserLoginAttempts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    TenancyName = table.Column<string>(maxLength: 64, nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    UserNameOrEmailAddress = table.Column<string>(maxLength: 256, nullable: true),
                    ClientIpAddress = table.Column<string>(maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(maxLength: 128, nullable: true),
                    BrowserInfo = table.Column<string>(maxLength: 512, nullable: true),
                    Result = table.Column<byte>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserLoginAttempts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    TenantNotificationId = table.Column<Guid>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserOrganizationUnits",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    OrganizationUnitId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserOrganizationUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUsers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    AuthenticationSource = table.Column<string>(maxLength: 64, nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    EmailAddress = table.Column<string>(maxLength: 256, nullable: true),
                    Password = table.Column<string>(maxLength: 128, nullable: false),
                    EmailConfirmationCode = table.Column<string>(maxLength: 328, nullable: true),
                    PasswordResetCode = table.Column<string>(maxLength: 328, nullable: true),
                    LockoutEndDateUtc = table.Column<DateTime>(nullable: true),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    IsLockoutEnabled = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 32, nullable: true),
                    IsPhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(maxLength: 128, nullable: true),
                    IsTwoFactorEnabled = table.Column<bool>(nullable: false),
                    IsEmailConfirmed = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: false),
                    NormalizedEmailAddress = table.Column<string>(maxLength: 256, nullable: false),
                    ConcurrencyStamp = table.Column<string>(maxLength: 128, nullable: true),
                    SignInToken = table.Column<string>(nullable: true),
                    NeedToChangeThePassword = table.Column<bool>(nullable: false),
                    SignInTokenExpireTimeUtc = table.Column<DateTime>(nullable: true),
                    ProfilePictureId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUsers_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpUsers_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpUsers_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpWebhookEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WebhookName = table.Column<string>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpWebhookEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpWebhookSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    WebhookUri = table.Column<string>(nullable: false),
                    Secret = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Webhooks = table.Column<string>(nullable: true),
                    Headers = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpWebhookSubscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseCategories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(maxLength: 250, nullable: true),
                    Code = table.Column<string>(maxLength: 250, nullable: true),
                    ImgUrl = table.Column<string>(maxLength: 250, nullable: true),
                    ParentId = table.Column<long>(nullable: true),
                    CourseCategoryId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseCategories_CourseCategories_CourseCategoryId",
                        column: x => x.CourseCategoryId,
                        principalTable: "CourseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseClassHours",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    Intro = table.Column<string>(maxLength: 512, nullable: true),
                    SortNumber = table.Column<long>(nullable: false),
                    CourseId = table.Column<long>(nullable: false),
                    CourseSectionId = table.Column<long>(nullable: false),
                    ResourceId = table.Column<string>(maxLength: 512, nullable: true),
                    ResourceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseClassHours", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(maxLength: 128, nullable: true),
                    Intro = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    CourseVideoType = table.Column<int>(nullable: false),
                    ImgUrl = table.Column<string>(maxLength: 256, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CourseCode = table.Column<string>(nullable: true),
                    BuyerQqGroup = table.Column<string>(maxLength: 128, nullable: true),
                    NotBuyerQqGroup = table.Column<string>(maxLength: 128, nullable: true),
                    RecommendCourse = table.Column<bool>(nullable: false),
                    GradeOfDifficulty = table.Column<decimal>(nullable: false),
                    CourseState = table.Column<int>(nullable: false),
                    ValidDays = table.Column<long>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseSections",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(maxLength: 250, nullable: true),
                    Intro = table.Column<string>(maxLength: 250, nullable: true),
                    Index = table.Column<int>(nullable: false),
                    CoursesId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataFileObject",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    Bytes = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataFileObject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DownloadLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Version = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    ProjectName = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LtmBannerAds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    Title = table.Column<string>(maxLength: 128, nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    ThumbImgUrl = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Weight = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Types = table.Column<string>(nullable: false),
                    ViewCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LtmBannerAds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LtmBlogrolls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    BlogrollTypeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: false),
                    Except = table.Column<bool>(nullable: false),
                    Recommend = table.Column<bool>(nullable: false),
                    Weight = table.Column<int>(nullable: false),
                    Logo = table.Column<string>(nullable: true),
                    IconName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LtmBlogrolls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LtmBlogrollTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LtmBlogrollTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LtmChatMessages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    TargetUserId = table.Column<long>(nullable: false),
                    TargetTenantId = table.Column<int>(nullable: true),
                    Message = table.Column<string>(maxLength: 4096, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Side = table.Column<int>(nullable: false),
                    ReadState = table.Column<int>(nullable: false),
                    ReceiverReadState = table.Column<int>(nullable: false),
                    SharedMessageId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LtmChatMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LtmComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    PostId = table.Column<Guid>(nullable: false),
                    RepliedCommentId = table.Column<Guid>(nullable: true),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LtmComments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LtmFriendships",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    FriendUserId = table.Column<long>(nullable: false),
                    FriendTenantId = table.Column<int>(nullable: true),
                    FriendUserName = table.Column<string>(maxLength: 256, nullable: false),
                    FriendTenancyName = table.Column<string>(nullable: true),
                    FriendProfilePictureId = table.Column<Guid>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LtmFriendships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LtmPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    BlogId = table.Column<Guid>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    CoverImage = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    HistoryContent = table.Column<string>(nullable: true),
                    ReadCount = table.Column<int>(nullable: false),
                    PostType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LtmPosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LtmSysFileInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    Dir = table.Column<bool>(nullable: false),
                    IsImg = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    DateDirctoryName = table.Column<string>(nullable: true),
                    ObjectType = table.Column<string>(nullable: true),
                    StorageClass = table.Column<string>(nullable: true),
                    TimeModified = table.Column<string>(nullable: true),
                    FileExt = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    FormattedSize = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    IsHidden = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(maxLength: 95, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LtmSysFileInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LtmTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    UsageCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LtmTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LtmWebSiteNotices",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    Content = table.Column<string>(maxLength: 3000, nullable: false),
                    ViewCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LtmWebSiteNotices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LtmWechatAppConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    AppId = table.Column<string>(maxLength: 250, nullable: true),
                    AppSecret = table.Column<string>(maxLength: 250, nullable: true),
                    Token = table.Column<string>(maxLength: 250, nullable: true),
                    EncodingAESKey = table.Column<string>(maxLength: 500, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    AppOrgId = table.Column<string>(maxLength: 250, nullable: true),
                    AppType = table.Column<int>(nullable: false),
                    QRCodeUrl = table.Column<string>(maxLength: 250, nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LtmWechatAppConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(nullable: true),
                    AboutMe = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Wechat = table.Column<string>(nullable: true),
                    QQ = table.Column<string>(nullable: true),
                    GitHub = table.Column<string>(nullable: true),
                    Gitee = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NeteaseOrderInfos",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNo = table.Column<string>(nullable: true),
                    OrderDate = table.Column<DateTime>(nullable: true),
                    ProductType = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    ProductDes = table.Column<string>(nullable: true),
                    OriginalPrice = table.Column<decimal>(nullable: false),
                    TransactionAmount = table.Column<decimal>(nullable: false),
                    BuyerName = table.Column<string>(nullable: true),
                    GeneralizeSource = table.Column<string>(nullable: true),
                    GeneralizeName = table.Column<string>(nullable: true),
                    CardAmount = table.Column<decimal>(nullable: false),
                    PlatformHB = table.Column<decimal>(nullable: false),
                    PracticalAmount = table.Column<decimal>(nullable: false),
                    PayType = table.Column<string>(nullable: true),
                    ThirdPartyPayServiceCharge = table.Column<decimal>(nullable: false),
                    GeneralizeServiceCharge = table.Column<decimal>(nullable: false),
                    PlatformServiceCharge = table.Column<decimal>(nullable: false),
                    RealityAmount = table.Column<decimal>(nullable: false),
                    TransactionStatus = table.Column<string>(nullable: true),
                    TransactionSuccessDate = table.Column<DateTime>(nullable: true),
                    SettleAccounts = table.Column<string>(nullable: true),
                    SettleAccountsDate = table.Column<DateTime>(nullable: true),
                    IsChecked = table.Column<bool>(nullable: false),
                    IsGiteeChecked = table.Column<bool>(nullable: false),
                    Platform = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NeteaseOrderInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    OrderCode = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    ProductId = table.Column<Guid>(nullable: true),
                    CourseId = table.Column<long>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Discounts = table.Column<decimal>(nullable: false),
                    ActualPayment = table.Column<decimal>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ProductCreateProjectCount = table.Column<int>(nullable: false),
                    ProductIndate = table.Column<double>(nullable: false),
                    UserId = table.Column<long>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    OrderSourceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PortalWebsiteSettings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    UrlAddress = table.Column<string>(nullable: true),
                    KeyWord = table.Column<string>(nullable: true),
                    Describe = table.Column<string>(nullable: true),
                    Copyright = table.Column<string>(nullable: true),
                    Contact = table.Column<string>(nullable: true),
                    OtherInfo = table.Column<string>(nullable: true),
                    TopCustomCodeSnippet = table.Column<string>(nullable: true),
                    BottomCustomCodeSnippet = table.Column<string>(nullable: true),
                    Addresser = table.Column<string>(nullable: true),
                    MailAddress = table.Column<string>(nullable: true),
                    MailPassword = table.Column<string>(nullable: true),
                    SMTP = table.Column<string>(nullable: true),
                    SendPort = table.Column<int>(nullable: false),
                    SendType = table.Column<int>(nullable: false),
                    AboutUsContent = table.Column<string>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortalWebsiteSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    CreateProjectCount = table.Column<int>(nullable: false),
                    Recommended = table.Column<bool>(nullable: false),
                    Indate = table.Column<double>(nullable: false),
                    Published = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductSecretKeys",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    SecretKey = table.Column<string>(nullable: true),
                    ProductId = table.Column<Guid>(nullable: false),
                    ProductCode = table.Column<string>(nullable: true),
                    Used = table.Column<bool>(nullable: false),
                    UserId = table.Column<long>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    OrderId = table.Column<Guid>(nullable: true),
                    OrderCode = table.Column<string>(nullable: true),
                    InSale = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSecretKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ImgUrl = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ShortName = table.Column<string>(nullable: true),
                    Format = table.Column<string>(nullable: true),
                    DefaultDocumentName = table.Column<string>(nullable: true),
                    NavigationDocumentName = table.Column<string>(nullable: true),
                    DocumentStoreType = table.Column<string>(nullable: true),
                    GoogleCustomSearchId = table.Column<string>(nullable: true),
                    ExtraProperties = table.Column<string>(nullable: true),
                    MainWebsiteUrl = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    LatestVersionBranchName = table.Column<string>(nullable: true),
                    MinimumVersion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TencentOrderInfos",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<long>(nullable: false),
                    OrderNumber = table.Column<long>(nullable: false),
                    CourseTitle = table.Column<string>(nullable: true),
                    CourseType = table.Column<string>(nullable: true),
                    CreationTime = table.Column<string>(nullable: true),
                    PurchaseTime = table.Column<string>(nullable: true),
                    UIn = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(nullable: true),
                    TradingStatus = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    RecipientPhone = table.Column<string>(nullable: true),
                    RecipientName = table.Column<string>(nullable: true),
                    RecipientAddress = table.Column<string>(nullable: true),
                    TradeType = table.Column<string>(nullable: true),
                    UsersPay = table.Column<decimal>(nullable: false),
                    PlatformSubsidies = table.Column<decimal>(nullable: false),
                    RefundAmount = table.Column<decimal>(nullable: false),
                    AmountPaid = table.Column<decimal>(nullable: false),
                    SettlementAmount = table.Column<decimal>(nullable: false),
                    SettlementRatio = table.Column<decimal>(nullable: false),
                    ChannelFee = table.Column<decimal>(nullable: false),
                    TrafficConversionFee = table.Column<decimal>(nullable: false),
                    AppleShares = table.Column<decimal>(nullable: false),
                    DistributorShare = table.Column<decimal>(nullable: false),
                    SettlementStatus = table.Column<string>(nullable: true),
                    IsIosOrder = table.Column<bool>(nullable: false),
                    IsDistributorOrder = table.Column<bool>(nullable: false),
                    OrderType = table.Column<string>(nullable: true),
                    RemarkOne = table.Column<string>(nullable: true),
                    RemarkTwo = table.Column<string>(nullable: true),
                    RemarkThree = table.Column<string>(nullable: true),
                    IsQqGroupChecked = table.Column<bool>(nullable: false),
                    IsGiteeChecked = table.Column<bool>(nullable: false),
                    Is52AbpChecked = table.Column<bool>(nullable: false),
                    Platform = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TencentOrderInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDownloadConfigs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCode = table.Column<string>(nullable: true),
                    DownloadType = table.Column<string>(nullable: true),
                    ResidueDegree = table.Column<long>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: true),
                    Indate = table.Column<double>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<long>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDownloadConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VideoResources",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    VideoId = table.Column<string>(nullable: true),
                    Tags = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: true),
                    Duration = table.Column<decimal>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ModificationTime = table.Column<string>(nullable: true),
                    SyncCreationTime = table.Column<string>(nullable: true),
                    CoverURL = table.Column<string>(nullable: true),
                    CateId = table.Column<long>(nullable: true),
                    CateName = table.Column<string>(nullable: true),
                    DownloadSwitch = table.Column<string>(nullable: true),
                    TemplateGroupId = table.Column<string>(nullable: true),
                    PreprocessStatus = table.Column<string>(nullable: true),
                    StorageLocation = table.Column<string>(nullable: true),
                    RegionId = table.Column<string>(nullable: true),
                    CustomMediaInfo = table.Column<string>(nullable: true),
                    AppId = table.Column<string>(nullable: true),
                    CreatorUser = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpDynamicEntityProperties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityFullName = table.Column<string>(nullable: true),
                    DynamicPropertyId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpDynamicEntityProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpDynamicEntityProperties_AbpDynamicProperties_DynamicPropertyId",
                        column: x => x.DynamicPropertyId,
                        principalTable: "AbpDynamicProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpDynamicPropertyValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    DynamicPropertyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpDynamicPropertyValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpDynamicPropertyValues_AbpDynamicProperties_DynamicPropertyId",
                        column: x => x.DynamicPropertyId,
                        principalTable: "AbpDynamicProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpFeatures",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(maxLength: 2000, nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    EditionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpFeatures_AbpEditions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "AbpEditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppSubscriptionPayments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Gateway = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    EditionId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    DayCount = table.Column<int>(nullable: false),
                    PaymentPeriodType = table.Column<int>(nullable: true),
                    PaymentId = table.Column<string>(nullable: true),
                    InvoiceNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSubscriptionPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSubscriptionPayments_AbpEditions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "AbpEditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityChanges",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangeTime = table.Column<DateTime>(nullable: false),
                    ChangeType = table.Column<byte>(nullable: false),
                    EntityChangeSetId = table.Column<long>(nullable: false),
                    EntityId = table.Column<string>(maxLength: 48, nullable: true),
                    EntityTypeFullName = table.Column<string>(maxLength: 192, nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpEntityChanges_AbpEntityChangeSets_EntityChangeSetId",
                        column: x => x.EntityChangeSetId,
                        principalTable: "AbpEntityChangeSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 64, nullable: false),
                    IsStatic = table.Column<bool>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    NormalizedName = table.Column<string>(maxLength: 32, nullable: false),
                    ConcurrencyStamp = table.Column<string>(maxLength: 128, nullable: true),
                    Description = table.Column<string>(maxLength: 5000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpRoles_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpRoles_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpRoles_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpSettings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpSettings_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpTenants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenancyName = table.Column<string>(maxLength: 64, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    ConnectionString = table.Column<string>(maxLength: 1024, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    EditionId = table.Column<int>(nullable: true),
                    SubscriptionEndUtc = table.Column<DateTime>(nullable: true),
                    IsInTrialPeriod = table.Column<bool>(nullable: false),
                    CustomCssId = table.Column<Guid>(nullable: true),
                    LogoId = table.Column<Guid>(nullable: true),
                    LogoFileType = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpTenants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpTenants_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpTenants_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpTenants_AbpEditions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "AbpEditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpTenants_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserClaims",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    ClaimType = table.Column<string>(maxLength: 256, nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserClaims_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserLogins",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserLogins_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserRoles_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserTokens",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    Value = table.Column<string>(maxLength: 512, nullable: true),
                    ExpireDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserTokens_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LtmBlogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ShortName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    BlogUserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LtmBlogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LtmBlogs_AbpUsers_BlogUserId",
                        column: x => x.BlogUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpWebhookSendAttempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WebhookEventId = table.Column<Guid>(nullable: false),
                    WebhookSubscriptionId = table.Column<Guid>(nullable: false),
                    Response = table.Column<string>(nullable: true),
                    ResponseStatusCode = table.Column<int>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpWebhookSendAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpWebhookSendAttempts_AbpWebhookEvents_WebhookEventId",
                        column: x => x.WebhookEventId,
                        principalTable: "AbpWebhookEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseToCourseCategories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    CourseId = table.Column<long>(nullable: false),
                    CourseCategoryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseToCourseCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseToCourseCategories_CourseCategories_CourseCategoryId",
                        column: x => x.CourseCategoryId,
                        principalTable: "CourseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseToCourseCategories_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseToStudent",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<long>(nullable: true),
                    StudentId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseToStudent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseToStudent_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseToStudent_AbpUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseToTeacher",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<long>(nullable: false),
                    TeacherId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseToTeacher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseToTeacher_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseToTeacher_AbpUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LtmPostTags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    PostId = table.Column<Guid>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LtmPostTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LtmPostTags_LtmPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "LtmPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseVideoResource",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    CourseId = table.Column<long>(nullable: false),
                    VideoResourceId = table.Column<long>(nullable: false),
                    VideoDisplayName = table.Column<string>(nullable: true),
                    SortNumber = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseVideoResource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseVideoResource_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseVideoResource_VideoResources_VideoResourceId",
                        column: x => x.VideoResourceId,
                        principalTable: "VideoResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpDynamicEntityPropertyValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(nullable: false),
                    EntityId = table.Column<string>(nullable: true),
                    DynamicEntityPropertyId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpDynamicEntityPropertyValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpDynamicEntityPropertyValues_AbpDynamicEntityProperties_DynamicEntityPropertyId",
                        column: x => x.DynamicEntityPropertyId,
                        principalTable: "AbpDynamicEntityProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityPropertyChanges",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityChangeId = table.Column<long>(nullable: false),
                    NewValue = table.Column<string>(maxLength: 512, nullable: true),
                    OriginalValue = table.Column<string>(maxLength: 512, nullable: true),
                    PropertyName = table.Column<string>(maxLength: 96, nullable: true),
                    PropertyTypeFullName = table.Column<string>(maxLength: 192, nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityPropertyChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpEntityPropertyChanges_AbpEntityChanges_EntityChangeId",
                        column: x => x.EntityChangeId,
                        principalTable: "AbpEntityChanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpPermissions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    IsGranted = table.Column<bool>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    RoleId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpPermissions_AbpRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AbpRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbpPermissions_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpRoleClaims",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(maxLength: 256, nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpRoleClaims_AbpRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AbpRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LtmBlogTag",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    BlogId = table.Column<Guid>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LtmBlogTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LtmBlogTag_LtmBlogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "LtmBlogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogs_TenantId_ExecutionDuration",
                table: "AbpAuditLogs",
                columns: new[] { "TenantId", "ExecutionDuration" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogs_TenantId_ExecutionTime",
                table: "AbpAuditLogs",
                columns: new[] { "TenantId", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogs_TenantId_UserId",
                table: "AbpAuditLogs",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpBackgroundJobs_IsAbandoned_NextTryTime",
                table: "AbpBackgroundJobs",
                columns: new[] { "IsAbandoned", "NextTryTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpDynamicEntityProperties_DynamicPropertyId",
                table: "AbpDynamicEntityProperties",
                column: "DynamicPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpDynamicEntityProperties_EntityFullName_DynamicPropertyId_TenantId",
                table: "AbpDynamicEntityProperties",
                columns: new[] { "EntityFullName", "DynamicPropertyId", "TenantId" },
                unique: true,
                filter: "[EntityFullName] IS NOT NULL AND [TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AbpDynamicEntityPropertyValues_DynamicEntityPropertyId",
                table: "AbpDynamicEntityPropertyValues",
                column: "DynamicEntityPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpDynamicProperties_PropertyName_TenantId",
                table: "AbpDynamicProperties",
                columns: new[] { "PropertyName", "TenantId" },
                unique: true,
                filter: "[PropertyName] IS NOT NULL AND [TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AbpDynamicPropertyValues_DynamicPropertyId",
                table: "AbpDynamicPropertyValues",
                column: "DynamicPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChanges_EntityChangeSetId",
                table: "AbpEntityChanges",
                column: "EntityChangeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChanges_EntityTypeFullName_EntityId",
                table: "AbpEntityChanges",
                columns: new[] { "EntityTypeFullName", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChangeSets_TenantId_CreationTime",
                table: "AbpEntityChangeSets",
                columns: new[] { "TenantId", "CreationTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChangeSets_TenantId_Reason",
                table: "AbpEntityChangeSets",
                columns: new[] { "TenantId", "Reason" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChangeSets_TenantId_UserId",
                table: "AbpEntityChangeSets",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityPropertyChanges_EntityChangeId",
                table: "AbpEntityPropertyChanges",
                column: "EntityChangeId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpFeatures_EditionId_Name",
                table: "AbpFeatures",
                columns: new[] { "EditionId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpFeatures_TenantId_Name",
                table: "AbpFeatures",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpLanguages_TenantId_Name",
                table: "AbpLanguages",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpLanguageTexts_TenantId_Source_LanguageName_Key",
                table: "AbpLanguageTexts",
                columns: new[] { "TenantId", "Source", "LanguageName", "Key" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpNotificationSubscriptions_NotificationName_EntityTypeName_EntityId_UserId",
                table: "AbpNotificationSubscriptions",
                columns: new[] { "NotificationName", "EntityTypeName", "EntityId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpNotificationSubscriptions_TenantId_NotificationName_EntityTypeName_EntityId_UserId",
                table: "AbpNotificationSubscriptions",
                columns: new[] { "TenantId", "NotificationName", "EntityTypeName", "EntityId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnitRoles_TenantId_OrganizationUnitId",
                table: "AbpOrganizationUnitRoles",
                columns: new[] { "TenantId", "OrganizationUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnitRoles_TenantId_RoleId",
                table: "AbpOrganizationUnitRoles",
                columns: new[] { "TenantId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_ParentId",
                table: "AbpOrganizationUnits",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_TenantId_Code",
                table: "AbpOrganizationUnits",
                columns: new[] { "TenantId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissions_TenantId_Name",
                table: "AbpPermissions",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissions_RoleId",
                table: "AbpPermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissions_UserId",
                table: "AbpPermissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpPersistedGrants_SubjectId_ClientId_Type",
                table: "AbpPersistedGrants",
                columns: new[] { "SubjectId", "ClientId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoleClaims_RoleId",
                table: "AbpRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoleClaims_TenantId_ClaimType",
                table: "AbpRoleClaims",
                columns: new[] { "TenantId", "ClaimType" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoles_CreatorUserId",
                table: "AbpRoles",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoles_DeleterUserId",
                table: "AbpRoles",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoles_LastModifierUserId",
                table: "AbpRoles",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoles_TenantId_NormalizedName",
                table: "AbpRoles",
                columns: new[] { "TenantId", "NormalizedName" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpSettings_UserId",
                table: "AbpSettings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpSettings_TenantId_Name_UserId",
                table: "AbpSettings",
                columns: new[] { "TenantId", "Name", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenantNotifications_TenantId",
                table: "AbpTenantNotifications",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_CreationTime",
                table: "AbpTenants",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_CreatorUserId",
                table: "AbpTenants",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_DeleterUserId",
                table: "AbpTenants",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_EditionId",
                table: "AbpTenants",
                column: "EditionId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_LastModifierUserId",
                table: "AbpTenants",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_SubscriptionEndUtc",
                table: "AbpTenants",
                column: "SubscriptionEndUtc");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_TenancyName",
                table: "AbpTenants",
                column: "TenancyName");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_EmailAddress",
                table: "AbpUserAccounts",
                column: "EmailAddress");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_UserName",
                table: "AbpUserAccounts",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_TenantId_EmailAddress",
                table: "AbpUserAccounts",
                columns: new[] { "TenantId", "EmailAddress" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_TenantId_UserId",
                table: "AbpUserAccounts",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_TenantId_UserName",
                table: "AbpUserAccounts",
                columns: new[] { "TenantId", "UserName" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserClaims_UserId",
                table: "AbpUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserClaims_TenantId_ClaimType",
                table: "AbpUserClaims",
                columns: new[] { "TenantId", "ClaimType" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLoginAttempts_UserId_TenantId",
                table: "AbpUserLoginAttempts",
                columns: new[] { "UserId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLoginAttempts_TenancyName_UserNameOrEmailAddress_Result",
                table: "AbpUserLoginAttempts",
                columns: new[] { "TenancyName", "UserNameOrEmailAddress", "Result" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLogins_UserId",
                table: "AbpUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLogins_TenantId_UserId",
                table: "AbpUserLogins",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLogins_TenantId_LoginProvider_ProviderKey",
                table: "AbpUserLogins",
                columns: new[] { "TenantId", "LoginProvider", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserNotifications_UserId_State_CreationTime",
                table: "AbpUserNotifications",
                columns: new[] { "UserId", "State", "CreationTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserOrganizationUnits_TenantId_OrganizationUnitId",
                table: "AbpUserOrganizationUnits",
                columns: new[] { "TenantId", "OrganizationUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserOrganizationUnits_TenantId_UserId",
                table: "AbpUserOrganizationUnits",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserRoles_UserId",
                table: "AbpUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserRoles_TenantId_RoleId",
                table: "AbpUserRoles",
                columns: new[] { "TenantId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserRoles_TenantId_UserId",
                table: "AbpUserRoles",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_CreatorUserId",
                table: "AbpUsers",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_DeleterUserId",
                table: "AbpUsers",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_LastModifierUserId",
                table: "AbpUsers",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_TenantId_NormalizedEmailAddress",
                table: "AbpUsers",
                columns: new[] { "TenantId", "NormalizedEmailAddress" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_TenantId_NormalizedUserName",
                table: "AbpUsers",
                columns: new[] { "TenantId", "NormalizedUserName" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserTokens_UserId",
                table: "AbpUserTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserTokens_TenantId_UserId",
                table: "AbpUserTokens",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpWebhookSendAttempts_WebhookEventId",
                table: "AbpWebhookSendAttempts",
                column: "WebhookEventId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSubscriptionPayments_EditionId",
                table: "AppSubscriptionPayments",
                column: "EditionId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseCategories_CourseCategoryId",
                table: "CourseCategories",
                column: "CourseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseToCourseCategories_CourseCategoryId",
                table: "CourseToCourseCategories",
                column: "CourseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseToCourseCategories_CourseId",
                table: "CourseToCourseCategories",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseToStudent_CourseId",
                table: "CourseToStudent",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseToStudent_StudentId",
                table: "CourseToStudent",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseToTeacher_CourseId",
                table: "CourseToTeacher",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseToTeacher_TeacherId",
                table: "CourseToTeacher",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseVideoResource_CourseId",
                table: "CourseVideoResource",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseVideoResource_VideoResourceId",
                table: "CourseVideoResource",
                column: "VideoResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_DataFileObject_TenantId",
                table: "DataFileObject",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LtmBlogs_BlogUserId",
                table: "LtmBlogs",
                column: "BlogUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LtmBlogTag_BlogId",
                table: "LtmBlogTag",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_LtmChatMessages_TargetTenantId_TargetUserId_ReadState",
                table: "LtmChatMessages",
                columns: new[] { "TargetTenantId", "TargetUserId", "ReadState" });

            migrationBuilder.CreateIndex(
                name: "IX_LtmChatMessages_TargetTenantId_UserId_ReadState",
                table: "LtmChatMessages",
                columns: new[] { "TargetTenantId", "UserId", "ReadState" });

            migrationBuilder.CreateIndex(
                name: "IX_LtmChatMessages_TenantId_TargetUserId_ReadState",
                table: "LtmChatMessages",
                columns: new[] { "TenantId", "TargetUserId", "ReadState" });

            migrationBuilder.CreateIndex(
                name: "IX_LtmChatMessages_TenantId_UserId_ReadState",
                table: "LtmChatMessages",
                columns: new[] { "TenantId", "UserId", "ReadState" });

            migrationBuilder.CreateIndex(
                name: "IX_LtmFriendships_FriendTenantId_FriendUserId",
                table: "LtmFriendships",
                columns: new[] { "FriendTenantId", "FriendUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_LtmFriendships_FriendTenantId_UserId",
                table: "LtmFriendships",
                columns: new[] { "FriendTenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_LtmFriendships_TenantId_FriendUserId",
                table: "LtmFriendships",
                columns: new[] { "TenantId", "FriendUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_LtmFriendships_TenantId_UserId",
                table: "LtmFriendships",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_LtmPostTags_PostId",
                table: "LtmPostTags",
                column: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpAuditLogs");

            migrationBuilder.DropTable(
                name: "AbpBackgroundJobs");

            migrationBuilder.DropTable(
                name: "AbpDynamicEntityPropertyValues");

            migrationBuilder.DropTable(
                name: "AbpDynamicPropertyValues");

            migrationBuilder.DropTable(
                name: "AbpEntityPropertyChanges");

            migrationBuilder.DropTable(
                name: "AbpFeatures");

            migrationBuilder.DropTable(
                name: "AbpLanguages");

            migrationBuilder.DropTable(
                name: "AbpLanguageTexts");

            migrationBuilder.DropTable(
                name: "AbpNotifications");

            migrationBuilder.DropTable(
                name: "AbpNotificationSubscriptions");

            migrationBuilder.DropTable(
                name: "AbpOrganizationUnitRoles");

            migrationBuilder.DropTable(
                name: "AbpOrganizationUnits");

            migrationBuilder.DropTable(
                name: "AbpPermissions");

            migrationBuilder.DropTable(
                name: "AbpPersistedGrants");

            migrationBuilder.DropTable(
                name: "AbpRoleClaims");

            migrationBuilder.DropTable(
                name: "AbpSettings");

            migrationBuilder.DropTable(
                name: "AbpTenantNotifications");

            migrationBuilder.DropTable(
                name: "AbpTenants");

            migrationBuilder.DropTable(
                name: "AbpUserAccounts");

            migrationBuilder.DropTable(
                name: "AbpUserClaims");

            migrationBuilder.DropTable(
                name: "AbpUserLoginAttempts");

            migrationBuilder.DropTable(
                name: "AbpUserLogins");

            migrationBuilder.DropTable(
                name: "AbpUserNotifications");

            migrationBuilder.DropTable(
                name: "AbpUserOrganizationUnits");

            migrationBuilder.DropTable(
                name: "AbpUserRoles");

            migrationBuilder.DropTable(
                name: "AbpUserTokens");

            migrationBuilder.DropTable(
                name: "AbpWebhookSendAttempts");

            migrationBuilder.DropTable(
                name: "AbpWebhookSubscriptions");

            migrationBuilder.DropTable(
                name: "AppSubscriptionPayments");

            migrationBuilder.DropTable(
                name: "CourseClassHours");

            migrationBuilder.DropTable(
                name: "CourseSections");

            migrationBuilder.DropTable(
                name: "CourseToCourseCategories");

            migrationBuilder.DropTable(
                name: "CourseToStudent");

            migrationBuilder.DropTable(
                name: "CourseToTeacher");

            migrationBuilder.DropTable(
                name: "CourseVideoResource");

            migrationBuilder.DropTable(
                name: "DataFileObject");

            migrationBuilder.DropTable(
                name: "DownloadLogs");

            migrationBuilder.DropTable(
                name: "LtmBannerAds");

            migrationBuilder.DropTable(
                name: "LtmBlogrolls");

            migrationBuilder.DropTable(
                name: "LtmBlogrollTypes");

            migrationBuilder.DropTable(
                name: "LtmBlogTag");

            migrationBuilder.DropTable(
                name: "LtmChatMessages");

            migrationBuilder.DropTable(
                name: "LtmComments");

            migrationBuilder.DropTable(
                name: "LtmFriendships");

            migrationBuilder.DropTable(
                name: "LtmPostTags");

            migrationBuilder.DropTable(
                name: "LtmSysFileInfos");

            migrationBuilder.DropTable(
                name: "LtmTags");

            migrationBuilder.DropTable(
                name: "LtmWebSiteNotices");

            migrationBuilder.DropTable(
                name: "LtmWechatAppConfigs");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "NeteaseOrderInfos");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "PortalWebsiteSettings");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductSecretKeys");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "TencentOrderInfos");

            migrationBuilder.DropTable(
                name: "UserDownloadConfigs");

            migrationBuilder.DropTable(
                name: "AbpDynamicEntityProperties");

            migrationBuilder.DropTable(
                name: "AbpEntityChanges");

            migrationBuilder.DropTable(
                name: "AbpRoles");

            migrationBuilder.DropTable(
                name: "AbpWebhookEvents");

            migrationBuilder.DropTable(
                name: "AbpEditions");

            migrationBuilder.DropTable(
                name: "CourseCategories");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "VideoResources");

            migrationBuilder.DropTable(
                name: "LtmBlogs");

            migrationBuilder.DropTable(
                name: "LtmPosts");

            migrationBuilder.DropTable(
                name: "AbpDynamicProperties");

            migrationBuilder.DropTable(
                name: "AbpEntityChangeSets");

            migrationBuilder.DropTable(
                name: "AbpUsers");
        }
    }
}

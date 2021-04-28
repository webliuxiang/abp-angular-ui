using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.ObjectMapping;
using Abp.Threading;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using LTMCompanyName.YoyoCmsTemplate.Notifications.AppMessage;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos.Enqueue;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Importing.Dto;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users.UserPolicy;
using Microsoft.AspNetCore.Identity;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Importing
{
    public class ImportUsersToExcelJob : BackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
    {
        private readonly RoleManager _roleManager;
        private readonly IUserListExcelDataReader _userListExcelDataReader;
        private readonly IInvalidUserExporter _invalidUserExporter;
        private readonly IUserPolicy _userPolicy;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAppMessage _appMessage;
        private readonly IDataFileObjectManager _dataFileObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;

        public UserManager UserManager { get; set; }

        public ImportUsersToExcelJob(
            RoleManager roleManager,
            IUserListExcelDataReader userListExcelDataReader,
            IInvalidUserExporter invalidUserExporter,
            IUserPolicy userPolicy,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            IPasswordHasher<User> passwordHasher,


            ILocalizationManager localizationManager,
            IObjectMapper objectMapper, IDataFileObjectManager dataFileObjectManager, IAppMessage appMessage)
        {
            _roleManager = roleManager;
            _userListExcelDataReader = userListExcelDataReader;
            _invalidUserExporter = invalidUserExporter;
            _userPolicy = userPolicy;
            _passwordValidators = passwordValidators;
            _passwordHasher = passwordHasher;

            _objectMapper = objectMapper;
            _dataFileObjectManager = dataFileObjectManager;
            _appMessage = appMessage;
            _localizationSource = localizationManager.GetSource(AppConsts.LocalizationSourceName);
        }

        [UnitOfWork]
        public override void Execute(ImportUsersFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var users = GetUserListFromExcelOrNull(args);
                if (users == null || !users.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateUsers(args, users);
            }
        }

        private List<ImportUserDto> GetUserListFromExcelOrNull(ImportUsersFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _dataFileObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _userListExcelDataReader.GetUsersFromExcel(file.Bytes);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateUsers(ImportUsersFromExcelJobArgs args, List<ImportUserDto> users)
        {
            var invalidUsers = new List<ImportUserDto>();

            foreach (var user in users)
            {
                if (user.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateUserAsync(user));
                    }
                    catch (UserFriendlyException exception)
                    {
                        user.Exception = exception.Message;
                        invalidUsers.Add(user);
                    }
                    catch (Exception exception)
                    {
                        user.Exception = exception.ToString();
                        invalidUsers.Add(user);
                    }
                }
                else
                {
                    invalidUsers.Add(user);
                }
            }

            AsyncHelper.RunSync(() => ProcessImportUsersResultAsync(args, invalidUsers));
        }

        private async Task CreateUserAsync(ImportUserDto input)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();

            if (tenantId.HasValue)
            {
                await _userPolicy.CheckMaxUserCountAsync(tenantId.Value);
            }

            var user = _objectMapper.Map<User>(input); //Passwords is not mapped (see mapping configuration)
            user.Password = input.Password;
            user.TenantId = tenantId;

            if (!input.Password.IsNullOrEmpty())
            {
                await UserManager.InitializeOptionsAsync(tenantId);
                foreach (var validator in _passwordValidators)
                {
                    (await validator.ValidateAsync(UserManager, user, input.Password)).CheckErrors();
                }

                user.Password = _passwordHasher.HashPassword(user, input.Password);
            }

            user.Roles = new List<UserRole>();
            var roleList = _roleManager.Roles.ToList();

            foreach (var roleName in input.AssignedRoleNames)
            {
                var correspondingRoleName = GetRoleNameFromDisplayName(roleName, roleList);
                var role = await _roleManager.GetRoleByNameAsync(correspondingRoleName);
                user.Roles.Add(new UserRole(tenantId, user.Id, role.Id));
            }

            (await UserManager.CreateAsync(user)).CheckErrors();
        }

        private async Task ProcessImportUsersResultAsync(ImportUsersFromExcelJobArgs args, List<ImportUserDto> invalidUsers)
        {
            if (invalidUsers.Any())
            {
                var file = _invalidUserExporter.ExportToFile(invalidUsers);
                await _appMessage.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appMessage.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllUsersSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportUsersFromExcelJobArgs args)
        {
            _appMessage.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToUserList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }

        private string GetRoleNameFromDisplayName(string displayName, List<Role> roleList)
        {
            return roleList.FirstOrDefault(
                        r => r.DisplayName?.ToLowerInvariant() == displayName?.ToLowerInvariant()
                    )?.Name;
        }
    }
}

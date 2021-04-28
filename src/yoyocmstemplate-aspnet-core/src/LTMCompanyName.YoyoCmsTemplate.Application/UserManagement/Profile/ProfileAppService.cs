using System;
using System.DrawingCore;
using System.IO;
using System.Threading.Tasks;
using Abp;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Localization;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.AppFolders;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects.DataTempCache;
using LTMCompanyName.YoyoCmsTemplate.Friendships;
using LTMCompanyName.YoyoCmsTemplate.Timing;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Profile.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Profile
{
    public class ProfileAppService : YoyoCmsTemplateAppServiceBase, IProfileAppService
    {
        private const int MaxProfilPictureBytes = 1048576; //1MB
        private readonly IAppFolder _appFolders;
        private readonly ICacheManager _cacheManager;
        private readonly IFriendshipManager _friendshipManager;
        private readonly IDataFileObjectManager _dataFileObjectManager;
        private readonly ITimeZoneService _timeZoneService;
        private readonly IDataTempFileCacheManager _dataTempFileCacheManager;

        public ProfileAppService(
            IAppFolder appFolders,
            ITimeZoneService timezoneService,
              IFriendshipManager friendshipManager,
            ICacheManager cacheManager, IDataFileObjectManager dataFileObjectManager, IDataTempFileCacheManager dataTempFileCacheManager)
        {
            _appFolders = appFolders;
            _timeZoneService = timezoneService;
            _friendshipManager = friendshipManager;
            _cacheManager = cacheManager;
            _dataFileObjectManager = dataFileObjectManager;
            _dataTempFileCacheManager = dataTempFileCacheManager;
        }

        /// <summary>
        /// 获取当前的个人信息
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public async Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit()
        {
            var user = await GetCurrentUserAsync();
            var userProfileEditDto = ObjectMapper.Map<CurrentUserProfileEditDto>(user);

            //如果支持多时区
            if (Clock.SupportsMultipleTimezone)
            {
                userProfileEditDto.Timezone = await SettingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);

                var defaultTimeZoneId = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.User, AbpSession.TenantId);
                if (userProfileEditDto.Timezone == defaultTimeZoneId)
                {
                    userProfileEditDto.Timezone = string.Empty;
                }
            }

            return userProfileEditDto;
        }

        public async Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input)
        {
            var user = await GetCurrentUserAsync();

            if (user.PhoneNumber != input.PhoneNumber)
                input.IsPhoneNumberConfirmed = false;
            else if (user.IsPhoneNumberConfirmed) input.IsPhoneNumberConfirmed = true;

            ObjectMapper.Map(input, user);
            CheckErrors(await UserManager.UpdateAsync(user));

            if (Clock.SupportsMultipleTimezone)
            {
                var defaultValue =
                    await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.User, AbpSession.TenantId);
                await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(),
                    TimingSettingNames.TimeZone, defaultValue);

                //if (input.Timezone.IsNullOrEmpty())
                //{
                //    var defaultValue = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.User, AbpSession.TenantId);
                //    await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), TimingSettingNames.TimeZone, defaultValue);
                //}
                //else
                //{
                //    await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), TimingSettingNames.TimeZone, input.Timezone);
                //}
            }
        }

        public async Task UpdateProfilePictureAsync(UpdateProfilePictureInput input)
        {
            byte[] byteArray;

            var imageBytes = _dataTempFileCacheManager.GetFile(input.FileToken);

            if (imageBytes == null)
            {
                throw new UserFriendlyException("没有找到符合这个图片的token信息: " + input.FileToken);
            }

            using (var bmpImage = new Bitmap(new MemoryStream(imageBytes)))
            {
                var width = (input.Width == 0 || input.Width > bmpImage.Width) ? bmpImage.Width : input.Width;
                var height = (input.Height == 0 || input.Height > bmpImage.Height) ? bmpImage.Height : input.Height;
                var bmCrop = bmpImage.Clone(new Rectangle(input.X, input.Y, width, height), bmpImage.PixelFormat);

                using (var stream = new MemoryStream())
                {
                    bmCrop.Save(stream, bmpImage.RawFormat);
                    byteArray = stream.ToArray();
                }
            }

            if (byteArray.Length > MaxProfilPictureBytes)
            {
                throw new UserFriendlyException(L("ResizedProfilePicture_Warn_SizeLimit", AppConsts.ResizedMaxProfilPictureBytesUserFriendlyValue));
            }

            var user = await UserManager.GetUserByIdAsync(AbpSession.GetUserId());

            if (user.ProfilePictureId.HasValue)
            {

                await _dataFileObjectManager.DeleteAsync(user.ProfilePictureId.Value);
            }

            var storedFile = new DataFileObject(AbpSession.TenantId, byteArray);
            await _dataFileObjectManager.SaveAsync(storedFile);

            user.ProfilePictureId = storedFile.Id;
        }


        public async Task ChangePassword(ChangePasswordInput input)
        {
            await UserManager.InitializeOptionsAsync(AbpSession.TenantId);

            var user = await GetCurrentUserAsync();
            CheckErrors(await UserManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword));
        }


        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }


        public async Task<GetProfilePictureOutput> GetFriendProfilePictureById(GetFriendProfilePictureByIdInput input)
        {
            if (!input.ProfilePictureId.HasValue || await _friendshipManager.GetFriendshipOrNullAsync(AbpSession.ToUserIdentifier(), new UserIdentifier(input.TenantId, input.UserId)) == null)
            {
                return new GetProfilePictureOutput(string.Empty);
            }

            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                var bytes = await GetProfilePictureByIdOrNull(input.ProfilePictureId.Value);
                if (bytes == null)
                {
                    return new GetProfilePictureOutput(string.Empty);
                }

                return new GetProfilePictureOutput(Convert.ToBase64String(bytes));
            }
        }


        public async Task<GetProfilePictureOutputDto> GetProfilePictureByIdAsync(Guid profilePictureId)
        {
            return await GetProfilePictureByIdInternal(profilePictureId);
        }

        /// <summary>
        ///     删除用户头像
        /// </summary>
        /// <param name="profilePictureId">头像ID</param>
        /// <returns></returns>
        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Administration_Users_DeleteProfilePicture)]
        public async Task DeleteProfilePictureById(Guid profilePictureId)
        {
            await _dataFileObjectManager.DeleteAsync(profilePictureId);
        }


        #region 私有函数

        /// <summary>
        ///    
        /// </summary>
        /// <param name="profilePictureId"></param>
        /// <returns></returns>
        private async Task<byte[]> GetProfilePictureByIdOrNull(Guid profilePictureId)
        {
            var file = await _dataFileObjectManager.GetOrNullAsync(profilePictureId);
            if (file == null) return null;

            return file.Bytes;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="profilePictureId"></param>
        /// <returns></returns>
        private async Task<GetProfilePictureOutputDto> GetProfilePictureByIdInternal(Guid profilePictureId)
        {
            var bytes = await GetProfilePictureByIdOrNull(profilePictureId);
            if (bytes == null) return new GetProfilePictureOutputDto(string.Empty);

            return new GetProfilePictureOutputDto(Convert.ToBase64String(bytes));
        }

        #endregion
    }
}

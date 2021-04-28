using System;
using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Profile.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Profile
{
    public interface IProfileAppService
    {
        /// <summary>
        /// 获取当前用户的信息进行编辑
        /// </summary>
        /// <returns> </returns>
        Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit();

        /// <summary>
        /// 更新当前用户信息
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input);

        /// <summary>
        /// 更新个人信息图片
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        Task UpdateProfilePictureAsync(UpdateProfilePictureInput input);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        Task ChangePassword(ChangePasswordInput input);

        /// <summary>
        /// 修改语言
        /// </summary>
        /// <param name="input"> </param>
        /// <returns> </returns>
        Task ChangeLanguage(ChangeUserLanguageDto input);

        /// <summary>
        /// 根据ID获取用户头像
        /// </summary>
        /// <param name="profilePictureId"> </param>
        /// <returns> </returns>
        Task<GetProfilePictureOutputDto> GetProfilePictureByIdAsync(Guid profilePictureId);

        /// <summary>
        /// 删除用户个人头像
        /// </summary>
        /// <param name="profilePictureId"> </param>
        /// <returns> </returns>
        Task DeleteProfilePictureById(Guid profilePictureId);
    }
}

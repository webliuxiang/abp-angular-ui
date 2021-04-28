namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Profile.Dtos
{
    /// <summary>
    /// 用户头像输出
    /// </summary>
    public class GetProfilePictureOutputDto
    {
        public GetProfilePictureOutputDto(string profilePicture)
        {
            ProfilePicture = profilePicture;
        }

        public string ProfilePicture { get; set; }
    }
}

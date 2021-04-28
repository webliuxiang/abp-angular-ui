namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Profile.Dtos
{
    public class GetProfilePictureOutput
    {
        public string ProfilePicture { get; set; }

        public GetProfilePictureOutput(string profilePicture)
        {
            ProfilePicture = profilePicture;
        }
    }
}

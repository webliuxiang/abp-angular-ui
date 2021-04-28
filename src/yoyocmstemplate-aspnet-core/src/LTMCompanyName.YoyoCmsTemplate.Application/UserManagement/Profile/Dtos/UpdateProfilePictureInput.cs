using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Profile.Dtos
{
    public class UpdateProfilePictureInput
    {
        [Required]
        [MaxLength(400)]
        public string FileToken { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}
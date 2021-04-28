using System;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Documents.Dtos
{
    [Serializable]
    public class DocumentContributorDto
    {
        public string Username { get; set; }

        public string UserProfileUrl { get; set; }

        public string AvatarUrl { get; set; }
    }
}

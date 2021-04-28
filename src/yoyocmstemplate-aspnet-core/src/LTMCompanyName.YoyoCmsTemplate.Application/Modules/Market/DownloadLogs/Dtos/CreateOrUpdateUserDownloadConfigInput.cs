using System.ComponentModel.DataAnnotations;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs.Dtos
{
    public class CreateOrUpdateUserDownloadConfigInput
    {
        [Required]
        public UserDownloadConfigEditDto UserDownloadConfig { get; set; }

    }
}

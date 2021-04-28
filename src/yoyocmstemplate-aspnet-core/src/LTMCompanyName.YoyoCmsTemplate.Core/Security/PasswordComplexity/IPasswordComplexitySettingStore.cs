using System.Threading.Tasks;

namespace LTMCompanyName.YoyoCmsTemplate.Security.PasswordComplexity
{
    public interface IPasswordComplexitySettingStore
    {
        Task<PasswordComplexitySetting> GetSettingsAsync();
    }

}

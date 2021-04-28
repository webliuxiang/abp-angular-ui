using System.Threading.Tasks;

namespace LTMCompanyName.YoyoCmsTemplate.Notifications.MailManage
{
    public interface IMailManager
    {
        Task SendMessage(string toMailAddress, string title, string body);
    }
}

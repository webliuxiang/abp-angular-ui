using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var webhost = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();


            return webhost;
        }
    }
}

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Startup
{
	public class Program
	{
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        }
    }
}

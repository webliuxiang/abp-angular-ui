using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Host.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseIIS()
                .UseIISIntegration()
                .UseStartup<Startup>()

                .Build();
        }
    }
}
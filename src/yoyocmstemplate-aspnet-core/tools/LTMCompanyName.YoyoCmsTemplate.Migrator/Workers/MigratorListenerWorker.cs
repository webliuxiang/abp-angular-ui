using BeetleX;
using BeetleX.EventArgs;

using Microsoft.Extensions.Hosting;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace LTMCompanyName.YoyoCmsTemplate.Migrator.Workers
{
    public class MigratorListenerWorker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            Console.WriteLine("启用监听 0.0.0.0:80");

            var server = SocketFactory.CreateTcpServer<MigratorListener>();
            server.Options.DefaultListen.Port = 80;
            server.Options.DefaultListen.Host = "0.0.0.0";
            server.Open();
        }



        internal class MigratorListener : ServerHandlerBase
        {
            public override void SessionReceive(IServer server, SessionReceiveEventArgs e)
            {
                e.Session.Stream.ToPipeStream().WriteLine(
                         $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} LTMCompanyName.YoyoCmsTemplate Migrator"
                         );
                e.Session.Stream.Flush();
                base.SessionReceive(server, e);
            }
        }
    }
}

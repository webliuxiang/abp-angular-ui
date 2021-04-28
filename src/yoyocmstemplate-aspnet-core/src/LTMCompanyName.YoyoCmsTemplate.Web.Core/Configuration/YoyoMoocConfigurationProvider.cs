using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Configuration;

namespace LTMCompanyName.YoyoCmsTemplate.Configuration
{

    /// <summary>
    /// 引入一个定时加载配置的功能
    /// </summary>
    public class YoyoMoocConfigurationProvider: ConfigurationProvider
    {


        Timer timer;

        public YoyoMoocConfigurationProvider() : base()
        {
            timer = new Timer();
            timer.Elapsed += Timer_Elapsed; //间隔后执行的方法
            timer.Interval = 3000;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
           
        }
    }
}

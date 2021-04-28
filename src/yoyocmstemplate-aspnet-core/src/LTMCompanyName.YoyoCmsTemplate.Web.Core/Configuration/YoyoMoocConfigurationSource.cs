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
    public class YoyoMoocConfigurationSource : IConfigurationSource
    {

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new YoyoMoocConfigurationProvider();
        }
    }
}

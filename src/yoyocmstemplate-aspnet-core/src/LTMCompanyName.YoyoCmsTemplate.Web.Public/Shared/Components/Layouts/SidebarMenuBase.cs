using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Shared.Components.Layouts
{
    /// <summary>
    /// 侧边栏组件
    /// </summary>
    public class SidebarMenuBase : ComponentBase
    {
       
    
    /// <summary>
        /// 获得 根模板页实例
        /// </summary>
        [CascadingParameter(Name = "Default")]
        public DefaultLayout? RootLayout { get; protected set; }

        /// <summary>
        /// 获得/设置 用户显示名称
        /// </summary>
        [Parameter]
        public string DisplayName { get; set; } = "";

        /// <summary>
        /// 获得/设置 用户显示名称改变事件回调方法
        /// </summary>
        [Parameter]
        public EventCallback<string> DisplayNameChanged { get; set; }

        /// <summary>
        /// 获得/设置 网站标题
        /// </summary>
        [Parameter]
        public string WebTitle { get; set; } = "";

        /// <summary>
        /// 获得/设置 网站标题改变事件回调方法
        /// </summary>
        [Parameter]
        public EventCallback<string> WebTitleChanged { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Shared.Components.Layouts
{
    public class DefaultLayoutComponentBase : LayoutComponentBase
    {
        /// <summary>
        ///注入Js
        /// </summary>
        [Inject]
        public IJSRuntime JSRuntime { get; set; }



        #region 组件

        /// <summary>
        /// Header 组件引用实例
        /// </summary>
        protected Header? Header { get; set; }

        /// <summary>
        /// SideBar 组件引用实例
        /// </summary>
        protected SidebarMenu? SidebarMenu { get; set; }

        /// <summary>
        /// Footer 组件引用实例
        /// </summary>
        protected Footer? Footer { get; set; }

        #endregion





        /// <summary>
        ///用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        public string DisplayName { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        public string WebTitle { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        public bool IsAdmin { get; set; }


        /// <summary>
        /// 获得/设置 系统首页
        /// </summary>
        public string HomeUrl { get; protected set; } = "Pages";

        /// <summary>
        /// 获得/设置 当前请求路径
        /// </summary>
        protected string RequestUrl { get; set; } = "";



        /// <summary>
        /// 网站标题变化时触发此方法
        /// </summary>
        /// <param name="title"></param>
        public void OnWebTitleChanged(string title)
        {
            Header?.WebTitleChanged.InvokeAsync(title);
            SidebarMenu?.WebTitleChanged.InvokeAsync(title);
        }


    }
}

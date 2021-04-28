using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Shared.Components.Layouts
{
    public class HeaderTopBase : AbpComponentBase
    {
        /// <summary>
        /// 获得 网站标题
        /// </summary>
        [Parameter]
        public string WebTitle { get; set; } = "";

        /// <summary>
        /// 获得/设置 网站标题改变事件回调方法
        /// </summary>
        [Parameter]
        public EventCallback<string> WebTitleChanged { get; set; }

        /// <summary>
        /// 获得 根模板页实例
        /// </summary>
        [CascadingParameter(Name = "Default")]
        protected DefaultLayout RootLayout { get; set; }

        /// <summary>
        /// 获得/设置 用户图标
        /// </summary>
        [Parameter]
        public string Icon { get; set; } = "";

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

        ///// <summary>
        ///// 获得/设置 是否显示 Blazor MVC 切换图标
        ///// </summary>
        //protected bool EnableBlazor { get; set; }

        /// <summary>
        /// 参数赋值方法
        /// </summary>
        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            //  EnableBlazor = DictHelper.RetrieveEnableBlazor();
            return base.SetParametersAsync(ParameterView.Empty);
        }




        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeAsync<object>("jQueryWidgets.load");
                await JSRuntime.InvokeAsync<object>("jQueryWidgets.initialize");
            }
        }

    }
}

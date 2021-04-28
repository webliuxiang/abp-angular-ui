using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Middlewares.Authorization
{
    public class AuthExceptionHandlingOption
    {
        /// <summary>
        /// 未登录重定向的地址,未配置则跳转到 DefaultRedirectPath
        /// </summary>
        public string NoLoginRedirectPath { get; set; }

        /// <summary>
        /// 没有权限重定向的地址,未配置则跳转到 DefaultRedirectPath
        /// </summary>
        public string NoPermissionRedirectPath { get; set; }

        /// <summary>
        /// 默认重定向的地址，默认为error
        /// </summary>
        public string DefaultRedirectPath { get; set; }
    }
}

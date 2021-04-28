using Abp.Application.Navigation;
using Abp.Localization;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Startup.Navigation
{
    public class PortalNavigationProvider : NavigationProvider
    {
        public const string MenuName = "Portal";

        public override void SetNavigation(INavigationProviderContext context)
        {
            var rootNode = new MenuDefinition(MenuName, L("PortalMenu"));
            context.Manager.Menus[MenuName] = rootNode;




            #region 首页

            var home = new MenuItemDefinition(
                         PortalNavPageNames.Home,
                         L("HomePage"),
                         url: "/"
                     );

            #endregion

            // 首页、下载、合作、学习、关于我们、更多。。。
            #region 文档

            var wiki = new MenuItemDefinition(
                PortalNavPageNames.Wiki,
                L("Wiki"),
                url: "Wiki"
            );

            #endregion


            #region 下载

            var download = new MenuItemDefinition(
                PortalNavPageNames.Download,
                L("Download"),
                url: "Download/Index"
            );

            #endregion

            #region 博客
            var blog = new MenuItemDefinition(
                PortalNavPageNames.Blog,
                L("Blog"),
                url: "Blog"
            );


            #endregion

            var sample = new MenuItemDefinition(PortalNavPageNames.Samples,
                new FixedLocalizableString("案例"),
                url: "Blog/samples");


            #region 教程

            var Partnership = new MenuItemDefinition(
                PortalNavPageNames.Partnership,
                  L("Partnership"),
                url: "About");

            var Learn = new MenuItemDefinition(
                PortalNavPageNames.Learn,
            L("Learn"), url: "");



            var college = new MenuItemDefinition(
                PortalNavPageNames.College,
                new FixedLocalizableString("视频课程"),
                url: "college"
            ); var purchase = new MenuItemDefinition(
                PortalNavPageNames.Pricing,
                new FixedLocalizableString("商业服务"),
                url: "Purchase"
            );
            var study163Video = new MenuItemDefinition(
                PortalNavPageNames.VideoCourse,
                new FixedLocalizableString("52ABP框架收费课程"),
                url: "https://study.163.com/course/courseMain.htm?courseId=1006191011&share=2&shareId=400000000309007", target: "_blank"
            );
            var study163Video2 = new MenuItemDefinition(
                PortalNavPageNames.VideoCourse,
                new FixedLocalizableString("ABP框架免费课程 "),
                url: "https://study.163.com/course/courseMain.htm?courseId=1005208064&share=2&shareId=400000000309007", target: "_blank"
            ); var study163VideoMore = new MenuItemDefinition(
                PortalNavPageNames.VideoCourse,
                new FixedLocalizableString("网易云课堂 "),
                url: "https://study.163.com/provider/400000000309007/index.htm?share=2&shareId=400000000309007", target: "_blank"
            );

            //var studyQqVideo = new MenuItemDefinition(
            //    PortalNavPageNames.VideoCourse,
            //    L("StudyQQVideo"),
            //    url: "http://52abp.ke.qq.com/", target: "_blank"
            //);


            //var faq = new MenuItemDefinition(
            //    PortalNavPageNames.Faq,
            //    L("Faq"),
            //    url: "/Blog/Faq"
            //);



            #endregion









            #region 菜单结构



            #endregion


            rootNode
                // 首页
                .AddItem(home)
                // 下载



                .AddItem(sample)
                   // .AddItem()

                   ;



        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AppConsts.LocalizationSourceName);
        }
    }
}
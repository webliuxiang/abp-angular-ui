using System.IO;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Json;

namespace LTMCompanyName.YoyoCmsTemplate.Localization
{
    public static class YoyoCmsTemplateLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            // 配置本地化资源

            LoadLocalizationFromFile(localizationConfiguration);


            //LoadLocalizationFromEmbeddedResource(localizationConfiguration);

        }

        /// <summary>
        /// 从 程序运行目录/Localization/SourceFiles/(Json/Xml) 中加载本地化资源文件 (推荐,修改后重启应用生效,无需编译)
        /// </summary>
        /// <param name="localizationConfiguration"></param>
        private static void LoadLocalizationFromFile(ILocalizationConfiguration localizationConfiguration)
        {
            var localizationResourceBasePath = Path.GetDirectoryName(typeof(YoyoCmsTemplateLocalizationConfigurer).Assembly.Location);

            // 使用Json资源(推荐)
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(AppConsts.LocalizationSourceName,
                    new JsonFileLocalizationDictionaryProvider(
                        Path.Combine(localizationResourceBasePath, "Localization", "SourceFiles", "jsons")
                    )
                )
            );

            //// 使用Xml资源
            //localizationConfiguration.Sources.Add(
            //    new DictionaryBasedLocalizationSource(AppConsts.LocalizationSourceName,
            //        new XmlFileLocalizationDictionaryProvider(
            //            Path.Combine(localizationResourceBasePath, "Localization", "SourceFiles", "Xml")
            //        )
            //    )
            //);
        }



    }
}

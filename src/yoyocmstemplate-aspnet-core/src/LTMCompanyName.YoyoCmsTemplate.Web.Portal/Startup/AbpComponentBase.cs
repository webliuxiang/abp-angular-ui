using System.Globalization;
using Abp;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Startup
{
    public class AbpComponentBase: ComponentBase
    {

        public AbpComponentBase()
        {
            LocalizationSourceName = AppConsts.LocalizationSourceName;

        }

        [Inject]

        public IUnitOfWorkManager UnitOfWorkManager { get; set; }


        /// <summary>
        ///注入Js
        /// </summary>
        [Inject]
        public IJSRuntime JSRuntime { get; set; }


        /// <summary>
        /// Gets current session information.
        /// </summary>
        [Inject]
        public IAbpSession AbpSession { get; set; }

        /// <summary>
        /// Reference to the setting manager.
        /// </summary>
        [Inject]

        public ISettingManager SettingManager { get; set; }

        /// <summary>
        /// Reference to the localization manager.
        /// </summary>
        [Inject]
        public ILocalizationManager LocalizationManager { get; set; }

        /// <summary>
        /// Gets/sets name of the localization source that is used in this application service.
        /// It must be set in order to use <see cref="L(string)"/> and <see cref="L(string,CultureInfo)"/> methods.
        /// </summary>
        protected string LocalizationSourceName { get; set; }

        /// <summary>
        /// Gets localization source.
        /// It's valid if <see cref="LocalizationSourceName"/> is set.
        /// </summary>
        protected ILocalizationSource LocalizationSource
        {
            get
            {
                if (LocalizationSourceName == null)
                {
                    throw new AbpException("Must set LocalizationSourceName before, in order to get LocalizationSource");
                }

                if (_localizationSource == null || _localizationSource.Name != LocalizationSourceName)
                {
                    _localizationSource = LocalizationManager.GetSource(LocalizationSourceName);
                }

                return _localizationSource;
            }
        }
        private ILocalizationSource _localizationSource;

        /// <summary>
        /// Reference to the logger to write logs.
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Reference to the object to object mapper.
        /// </summary>
        public IObjectMapper ObjectMapper { get; set; }

        /// <summary>
        /// Reference to the permission manager.
        /// </summary>
        public IPermissionManager PermissionManager { get; set; }

        /// <summary>
        /// Reference to the permission checker.
        /// </summary>
        public IPermissionChecker PermissionChecker { get; set; }

        /// <summary>
        /// Reference to the feature manager.
        /// </summary>
        public IFeatureManager FeatureManager { get; set; }

        /// <summary>
        /// Reference to the permission checker.
        /// </summary>
        public IFeatureChecker FeatureChecker { get; set; }

      

        /// <summary>
        /// Gets localized string for given key name and current language.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <returns>Localized string</returns>
        protected virtual string L(string name)
        {
            return LocalizationSource.GetString(name);
        }

        /// <summary>
        /// Gets localized string for given key name and current language with formatting strings.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <param name="args">Format arguments</param>
        /// <returns>Localized string</returns>
        protected string L(string name, params object[] args)
        {
            return LocalizationSource.GetString(name, args);
        }

        /// <summary>
        /// Gets localized string for given key name and specified culture information.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <param name="culture">culture information</param>
        /// <returns>Localized string</returns>
        protected virtual string L(string name, CultureInfo culture)
        {
            return LocalizationSource.GetString(name, culture);
        }

        /// <summary>
        /// Gets localized string for given key name and current language with formatting strings.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <param name="culture">culture information</param>
        /// <param name="args">Format arguments</param>
        /// <returns>Localized string</returns>
        protected string L(string name, CultureInfo culture, params object[] args)
        {
            return LocalizationSource.GetString(name, culture, args);
        }

    }
}

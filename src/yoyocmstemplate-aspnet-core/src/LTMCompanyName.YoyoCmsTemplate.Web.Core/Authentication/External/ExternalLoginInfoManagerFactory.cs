using Abp.Dependency;

namespace LTMCompanyName.YoyoCmsTemplate.Authentication.External
{
    public class ExternalLoginInfoManagerFactory : ITransientDependency
    {
        private readonly IIocManager _iocManager;

        public ExternalLoginInfoManagerFactory(IIocManager iocManager)
        {
            _iocManager = iocManager;
        }

        public IDisposableDependencyObjectWrapper<IExternalLoginInfoManager> GetExternalLoginInfoManager(string loginProvider)
        {
            if (loginProvider == "WsFederation")
            {
                return _iocManager.ResolveAsDisposable<WsFederationExternalLoginInfoManager>();
            }

            if (loginProvider == "QQ")
            {
                return _iocManager.ResolveAsDisposable<QQExternalLoginInfoManager>();
            }

            return _iocManager.ResolveAsDisposable<DefaultExternalLoginInfoManager>();
        }
    }
}
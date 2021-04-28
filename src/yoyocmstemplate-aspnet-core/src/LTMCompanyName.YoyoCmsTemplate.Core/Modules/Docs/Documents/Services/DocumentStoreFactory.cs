using System;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Dependency;
using L._52ABP.Common.Extensions;
using LTMCompanyName.YoyoCmsTemplate.Modules.Docs.Gitlab.Documents;
using LTMCompanyName.YoyoCmsTemplate.Modules.yoyo.Docs.Documents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using YoYo.ABPCommunity.Docs.Documents.FileSystem.Documents;
using YoYo.ABPCommunity.Docs.GitHub.Documents;

namespace YoYo.ABPCommunity.Docs.Documents.Services
{
    public class DocumentStoreFactory : IDocumentStoreFactory, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;
       

        public DocumentStoreFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
             
        }

        /// <summary>
        ///     根据文档类型不同调用不同的仓储工厂服务
        /// </summary>
        /// <param name="documentStoreType"></param>
        /// <returns></returns>
        public IDocumentStore Create(string documentStoreType)
        {

            documentStoreType = documentStoreType.ToLower();
            //todo:初始化文档类型,统一变成小写。
            var options = new DocumentStoreOptions();
            options.Stores.Add(GithubDocumentStore.Type.ToLower(), typeof(GithubDocumentStore));
            options.Stores.Add(FileSystemDocumentStore.Type.ToLower(), typeof(FileSystemDocumentStore));
            options.Stores.Add(GitlabDocumentStore.Type.ToLower(), typeof(GitlabDocumentStore));

            var serviceType = options.Stores.GetOrDefault(documentStoreType);


            if (serviceType == null)
            {
                throw new ApplicationException($"当前文档类型的仓储服务不存在: {documentStoreType}");
            }

            var service = (IDocumentStore)_serviceProvider.GetRequiredService(serviceType);

            return service;


        }

        public TDocumentStore Create<TDocumentStore>() where TDocumentStore : IDocumentStore
        {
            return _serviceProvider.GetRequiredService<TDocumentStore>();
        }
    }
}

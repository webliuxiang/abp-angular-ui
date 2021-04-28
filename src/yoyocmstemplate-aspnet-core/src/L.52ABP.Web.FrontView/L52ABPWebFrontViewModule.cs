using System.Reflection;
using Abp.AspNetCore;
using Abp.Modules;
using Abp.Reflection.Extensions;
using L._52ABP.Core;
using LTMCompanyName.YoyoCmsTemplate;

namespace L._52ABP.Web.FrontView
{
	[DependsOn(typeof(YoyoCmsTemplateWebCoreModule))]
	public class L52ABPWebFrontViewModule : AbpModule
	{
		public override void PreInitialize()
		{
		}

		public override void Initialize()
		{
		//	IocManager.RegisterAssemblyByConvention(typeof(L52ABPWebFrontViewModule).GetAssembly());

		 IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
		}

		public override void PostInitialize()
		{
		}
	}
}

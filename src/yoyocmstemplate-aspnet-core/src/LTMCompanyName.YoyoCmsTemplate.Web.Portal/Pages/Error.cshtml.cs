using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Portal.Pages
{
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public class ErrorModel : YoyoCmsTemplatePageModel
    {
		public string RequestId { get; set; }

		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public ErrorModel()
        {

        }

		public void OnGet(string statusCode)
		{
			RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
		}
	}
}

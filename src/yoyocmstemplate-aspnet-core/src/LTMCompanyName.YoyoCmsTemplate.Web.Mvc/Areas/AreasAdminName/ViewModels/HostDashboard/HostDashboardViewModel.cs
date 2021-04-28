namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.HostDashboard
{
    public class HostDashboardViewModel
    {
        public int ReportOnLoadDayCount { get; set; }

        public HostDashboardViewModel(int reportOnLoadDayCount)
        {
            ReportOnLoadDayCount = reportOnLoadDayCount;
        }
    }
}
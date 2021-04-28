namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.OrganizationUnits
{
    public class CreateOrganizationUnitModalViewModel
    {
        public long? ParentId { get; set; }

        public CreateOrganizationUnitModalViewModel(long? parentId)
        {
            ParentId = parentId;
        }
    }
}
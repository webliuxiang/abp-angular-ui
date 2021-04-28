using System.Collections.Generic;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Exporting
{
    public interface IUserListExcelExporter

    {

        FileDto ExportToExcel(List<UserListDto> userListDtos);
    }
}

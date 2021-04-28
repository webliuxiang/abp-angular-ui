using System.Collections.Generic;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Importing.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}

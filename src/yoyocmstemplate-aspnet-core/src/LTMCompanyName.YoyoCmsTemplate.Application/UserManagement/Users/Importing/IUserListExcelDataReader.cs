using System.Collections.Generic;
using Abp.Dependency;
using LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Importing.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Importing
{
    public interface IUserListExcelDataReader : ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}

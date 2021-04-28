using System.Collections.Generic;
using Abp.Dependency;
using LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.Excel.Importing.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.Excel.Importing
{
    public interface ITencentOrderExcelDataReader: ITransientDependency



    {



        List<ImportTencentOrderDto> GetTencentOrdersFromExcel(byte[] fileBytes);



    }
}

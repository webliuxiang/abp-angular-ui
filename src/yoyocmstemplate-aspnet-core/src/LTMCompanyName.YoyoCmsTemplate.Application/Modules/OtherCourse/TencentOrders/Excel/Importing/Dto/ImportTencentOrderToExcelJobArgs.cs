using System;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.TencentOrders.Excel.Importing.Dto
{

    [Serializable]
    public class ImportTencentOrderToExcelJobArgs
    {

        /// <summary>
        /// 数据库中的二进制对象
        /// </summary>
        public Guid DataFileObjectId { get; set; }

      //  public DataFileObject DataFileObject { get; set; }



    }
}

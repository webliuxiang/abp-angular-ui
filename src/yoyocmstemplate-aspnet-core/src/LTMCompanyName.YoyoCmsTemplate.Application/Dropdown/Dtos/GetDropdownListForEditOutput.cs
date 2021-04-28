
using System.Collections.Generic;

namespace LTMCompanyName.YoyoCmsTemplate.Dropdown.Dtos
{
    /// <summary>
    /// 读取可编辑下拉组件的Dto
    /// </summary>
    public class GetDropdownListForEditOutput
    {

        public DropdownListEditDto DropdownList { get; set; }
        public List<KeyValuePair<string, string>> DropdownTypeTypeEnum { get; set; }
    }
}

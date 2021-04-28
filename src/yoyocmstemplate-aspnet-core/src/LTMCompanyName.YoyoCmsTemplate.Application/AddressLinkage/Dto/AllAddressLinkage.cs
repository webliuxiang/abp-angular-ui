using System.Collections.Generic;

namespace LTMCompanyName.YoyoCmsTemplate.AddressLinkage.Dto
{
    /// <summary>
    /// 所有联动数据
    /// </summary>
    public class AllAddressLinkage
    {
        /// <summary>
        /// 省
        /// </summary>
        public List<AddressProvincetDto> Provinces { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public List<AddressCityDto> Citys { get; set; }


        /// <summary>
        /// 县
        /// </summary>
        public List<AddressAreaDto> Areas { get; set; }

        /// <summary>
        /// 镇
        /// </summary>
        public List<AddressStreetDto> Streets { get; set; }
    }
}
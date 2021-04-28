using System.Collections.Generic;
using LTMCompanyName.YoyoCmsTemplate.AddressLinkage.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.AddressLinkage
{
    public class AddressLinkageAppService : YoyoCmsTemplateAppServiceBase
    {
        private readonly AddressLinkageManager _addressLinkageManager;

        public AddressLinkageAppService(AddressLinkageManager addressLinkageManager)
        {
            _addressLinkageManager = addressLinkageManager;
        }

        /// <summary>
        /// 通过父级code获取省市区县镇数据
        /// </summary>
        /// <param name="addressEnum"> </param>
        /// <param name="parentCode"> 父级code </param>
        public List<AddressProvincetDto> GetByParentCode(AddressEnum addressEnum, string parentCode)
        {
            return _addressLinkageManager.GetAddressData(addressEnum, parentCode);
        }

        /// <summary>
        /// 通过code获取省市区县镇名称（没有code传空）
        /// </summary>
        /// <returns> </returns>
        public GetByCodeOutput GetByCode(GetByCodeInput input)
        {
            return _addressLinkageManager.GetByCode(input);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns> </returns>
        public AllAddressLinkage GetAll()
        {
            AllAddressLinkage ret = new AllAddressLinkage
            {
                Provinces = _addressLinkageManager.GetAddressData(AddressEnum.Provinces, ""),
                Citys = _addressLinkageManager.GetAllCity(),
                Areas = _addressLinkageManager.GetAllArea(),
                Streets = _addressLinkageManager.GetAllStreet()
            };
            return ret;
        }

        /// <summary>
        /// 获取所有市数据
        /// </summary>
        /// <returns> </returns>
        public List<AddressCityDto> GetAllCity()
        {
            return _addressLinkageManager.GetAllCity();
        }

        /// <summary>
        /// 获取所有县数据
        /// </summary>
        /// <returns> </returns>
        public List<AddressAreaDto> GetAllArea()
        {
            return _addressLinkageManager.GetAllArea();
        }

        /// <summary>
        /// 获取所有镇数据
        /// </summary>
        /// <returns> </returns>
        public List<AddressStreetDto> GetAllStreet()
        {
            return _addressLinkageManager.GetAllStreet();
        }
    }
}
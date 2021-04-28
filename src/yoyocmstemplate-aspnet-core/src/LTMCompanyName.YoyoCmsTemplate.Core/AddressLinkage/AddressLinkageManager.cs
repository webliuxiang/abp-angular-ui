using System;
using System.Collections.Generic;
using System.IO;
using Abp.Domain.Services;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.AddressLinkage.Dto;
using LTMCompanyName.YoyoCmsTemplate.DataFileObjects;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace LTMCompanyName.YoyoCmsTemplate.AddressLinkage
{
    public class AddressLinkageManager : DomainService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AddressLinkageManager(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 获取联动数据
        /// </summary>
        /// <param name="type">级别类型</param>
        /// <param name="code">上级code</param>
        /// <returns></returns>
        public List<AddressProvincetDto> GetAddressData(AddressEnum type, string code)
        {
            //读取wwwroot路径
            string webRootPath = _hostingEnvironment.WebRootPath;

            switch (type)
            {
                case AddressEnum.Provinces:
                    string provincesStr = File.ReadAllText($"{webRootPath}{AppConsts.AddressLinkageProvincesPath}");
                    return JsonConvert.DeserializeObject<List<AddressProvincetDto>>(provincesStr);
                case AddressEnum.Cities:
                    string citStr = File.ReadAllText($"{webRootPath}{AppConsts.AddressLinkageCitiesPath}");
                    var cityData = JsonConvert.DeserializeObject<List<AddressCityDto>>(citStr);
                    var retCity = cityData.FindAll(x => x.ProvinceCode.Equals(code));
                    return ObjectMapper.Map<List<AddressProvincetDto>>(retCity);
                case AddressEnum.Areas:
                    string areaStr = File.ReadAllText($"{webRootPath}{AppConsts.AddressLinkageAreasPath}");
                    var areaData = JsonConvert.DeserializeObject<List<AddressAreaDto>>(areaStr);
                    var retArea = areaData.FindAll(x => x.CityCode.Equals(code));
                    return ObjectMapper.Map<List<AddressProvincetDto>>(retArea);
                case AddressEnum.Streets:
                    string streetStr = File.ReadAllText($"{webRootPath}{AppConsts.AddressLinkageStreetsPath}");
                    var streetData = JsonConvert.DeserializeObject<List<AddressStreetDto>>(streetStr);
                    var retStreet = streetData.FindAll(x => x.AreaCode.Equals(code));
                    return ObjectMapper.Map<List<AddressProvincetDto>>(retStreet);
                default:
                    throw new UserFriendlyException("错误!", "正在修复中，请稍后重试...");
            }
        }

        /// <summary>
        /// 通过code获取省市区县镇名称
        /// </summary>
        /// <returns></returns>
        public GetByCodeOutput GetByCode(GetByCodeInput input)
        {
            GetByCodeOutput ret = new GetByCodeOutput
            {
                ProvinceName = string.IsNullOrEmpty(input.ProvinceCode) ? "" : GetAddressData(AddressEnum.Provinces, "").Find(x => x.Code.Equals(input.ProvinceCode)).Name,
                CityName = string.IsNullOrEmpty(input.CityCode) ? "" : GetAllCity().Find(x => x.Code.Equals(input.CityCode)).Name,
                AreaName = string.IsNullOrEmpty(input.AreaCode) ? "" : GetAllArea().Find(x => x.Code.Equals(input.AreaCode)).Name,
                StreetName = string.IsNullOrEmpty(input.StreetCode) ? "" : GetAllStreet().Find(x => !x.Code.Equals(input.StreetCode)).Name
            };
            return ret;
        }

        /// <summary>
        /// 获取所有市数据
        /// </summary>
        /// <returns></returns>
        public List<AddressCityDto> GetAllCity()
        {   //读取wwwroot路径
            string webRootPath = _hostingEnvironment.WebRootPath;
            string citStr = File.ReadAllText($"{webRootPath}{AppConsts.AddressLinkageCitiesPath}");
            return JsonConvert.DeserializeObject<List<AddressCityDto>>(citStr);
        }

        /// <summary>
        /// 获取所有县数据
        /// </summary>
        /// <returns></returns>
        public List<AddressAreaDto> GetAllArea()
        {   //读取wwwroot路径
            string webRootPath = _hostingEnvironment.WebRootPath;
            string areaStr = File.ReadAllText($"{webRootPath}{AppConsts.AddressLinkageAreasPath}");
            return JsonConvert.DeserializeObject<List<AddressAreaDto>>(areaStr);
        }

        /// <summary>
        /// 获取所有镇数据
        /// </summary>
        /// <returns></returns>
        public List<AddressStreetDto> GetAllStreet()
        {   //读取wwwroot路径
            string webRootPath = _hostingEnvironment.WebRootPath;
            string streetStr = File.ReadAllText($"{webRootPath}{AppConsts.AddressLinkageStreetsPath}");
            return JsonConvert.DeserializeObject<List<AddressStreetDto>>(streetStr);
        }

        public static implicit operator AddressLinkageManager(DataFileObjectManager v)
        {
            throw new NotImplementedException();
        }
    }
}

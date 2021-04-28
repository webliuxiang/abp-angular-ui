using System.Collections.Generic;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Editions.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Mvc.Areas.AreasAdminName.ViewModels.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        List<FlatFeatureDto> Features { get; set; }
    }
}
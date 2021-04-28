

using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace LTMCompanyName.YoyoCmsTemplate.Dtos
{
    public class PagedAndFilteredInputDto : IPagedResultRequest
    {
        [Range(1, AppConsts.MaxPageSize)]
        public int MaxResultCount { get; set; }

        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }

        public string FilterText { get; set; }

        public PagedAndFilteredInputDto()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
    }
}
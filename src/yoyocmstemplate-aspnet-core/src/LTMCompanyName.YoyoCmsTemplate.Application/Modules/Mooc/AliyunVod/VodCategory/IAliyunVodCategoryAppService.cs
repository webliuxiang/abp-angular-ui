using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Aliyun.Acs.vod.Model.V20170321;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.AliyunVod.VodCategory.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.AliyunVod.VodCategory
{
    public interface IAliyunVodCategoryAppService : IApplicationService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<VodCategoryEditDto> CreateVodCategory(VodCategoryEditDto input);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateVodCategory(VodCategoryEditDto input);

        Task DeleteVodCategory(EntityDto<long> input);

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        Task<List<VodCategoryEditDto>> GetAllVodCategories();

    }
}

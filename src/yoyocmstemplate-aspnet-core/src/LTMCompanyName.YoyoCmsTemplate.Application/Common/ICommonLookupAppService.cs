using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using L._52ABP.Application.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Common.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Editions.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

        /// <summary>
        /// 获取所有验证码支持的类型
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<ComboboxItemDtoT<int>>> GetValidateCodeTypesForCombobox();

        Task<PagedResultDto<NameValueDto>> FindUsers(CommonLookupFindUsersInput input);

        GetDefaultEditionNameOutput GetDefaultEditionName();


        /// <summary>
        /// 查找用户,Value 是 UserName
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<NameValueDto>> FindUsersSetUserNameToValue(CommonLookupFindUsersInput input);

        /// <summary>
        /// 获取枚举，字符串类型combox数据
        /// </summary>
        /// <param name="input">枚举类型,字符串,区分大小写</param>
        /// <returns></returns>
        Task<ListResultDto<ComboboxItemDtoT<string>>> GetEnumForCombobox(string input);
    }
}

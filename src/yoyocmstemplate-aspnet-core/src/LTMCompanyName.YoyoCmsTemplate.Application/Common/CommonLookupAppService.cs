using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.Linq.Extensions;

using L._52ABP.Application.Dtos;
using L._52ABP.Common.Extensions.Enums;
using L._52ABP.Core.VerificationCodeStore;

using LTMCompanyName.YoyoCmsTemplate.Common.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Editions;
using LTMCompanyName.YoyoCmsTemplate.Editions.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Extension;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseClassHours;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Common
{
    [AbpAuthorize]
    public class CommonLookupAppService : YoyoCmsTemplateAppServiceBase, ICommonLookupAppService
    {
        private readonly EditionManager _editionManager;
        readonly IOrderManager _orderManager;
        readonly IProductManager _productManager;
        readonly IEnumExtensionsAppService _enumExtensionsAppService;

        public CommonLookupAppService(EditionManager editionManager, IOrderManager orderManager, IProductManager productManager, IEnumExtensionsAppService enumExtensionsAppService)
        {
            _editionManager = editionManager;
            _orderManager = orderManager;
            _productManager = productManager;
            _enumExtensionsAppService = enumExtensionsAppService;
        }

        public async Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false)
        {



            var subscribableEditions = (await _editionManager.Editions.Cast<SubscribableEdition>().ToListAsync())
              .WhereIf(onlyFreeItems, e => e.IsFree)
              .OrderBy(e => e.MonthlyPrice);

            return new ListResultDto<SubscribableEditionComboboxItemDto>(
                subscribableEditions.Select(e => new SubscribableEditionComboboxItemDto(e.Id.ToString(), e.DisplayName, e.IsFree)).ToList()
            );
        }

        public async Task<PagedResultDto<NameValueDto>> FindUsers(CommonLookupFindUsersInput input)
        {
            if (AbpSession.TenantId != null)
            {


                //Prevent tenants to get other tenant's users.
                input.TenantId = AbpSession.TenantId;
            }

            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                var query = UserManager.Users
                    .WhereIf(
                        !input.FilterText.IsNullOrWhiteSpace(),
                        u =>
                            //u.Name.Contains(input.FilterText) ||
                            //u.Surname.Contains(input.FilterText) ||
                            u.UserName.Contains(input.FilterText) ||
                            u.EmailAddress.Contains(input.FilterText)
                    );

                var userCount = await query.CountAsync();
                var users = await query
                    .OrderBy(u => u.UserName)
                    //.OrderBy(u => u.Name)
                    //.ThenBy(u => u.Surname)
                    .PageBy(input)
                    .ToListAsync();

                return new PagedResultDto<NameValueDto>(
                    userCount,
                    users.Select(u =>
                        new NameValueDto(
                            u.FullName + " (" + u.EmailAddress + ")",
                            u.Id.ToString()
                            )
                        ).ToList()
                    );
            }
        }

        public GetDefaultEditionNameOutput GetDefaultEditionName()
        {
            return new GetDefaultEditionNameOutput
            {
                Name = EditionManager.DefaultEditionName
            };
        }

        public async Task<PagedResultDto<NameValueDto>> FindUsersSetUserNameToValue(CommonLookupFindUsersInput input)
        {
            if (AbpSession.TenantId != null)
            {
                // 防止租户获取其他租户的用户
                input.TenantId = AbpSession.TenantId;
            }

            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                var query = UserManager.Users
                    .WhereIf(
                        !input.FilterText.IsNullOrWhiteSpace(),
                        u =>
                            u.UserName.Contains(input.FilterText) ||
                            u.EmailAddress.Contains(input.FilterText)
                    );

                var userCount = await query.CountAsync();
                var users = query.ToList()
                    .OrderBy(u => u.Name)
                    .ThenBy(u => u.Surname)
                    .Take(input.MaxResultCount)
                    .Skip(input.SkipCount)
                    .ToList();

                return new PagedResultDto<NameValueDto>(
                    userCount,
                    users.Select(u =>
                        new NameValueDto(
                            u.UserName + " (" + u.EmailAddress + ")",
                            u.UserName
                        )
                    ).ToList()
                );
            }
        }

        public async Task<ListResultDto<ComboboxItemDtoT<int>>> GetValidateCodeTypesForCombobox()
        {
            await Task.Yield();

            var resList = new List<ComboboxItemDtoT<int>>
            {
                new ComboboxItemDtoT<int>((int)ValidateCodeType.Number, "数字"),
                new ComboboxItemDtoT<int>((int)ValidateCodeType.English, "英文"),
                new ComboboxItemDtoT<int>((int)ValidateCodeType.NumberAndLetter, "数字 + 英文"),
                new ComboboxItemDtoT<int>((int)ValidateCodeType.Hanzi, "汉字")
            };

            return new ListResultDto<ComboboxItemDtoT<int>>(resList);
        }



        /// <summary>
        /// 获取枚举，字符串类型combox数据
        /// </summary>
        /// <param name="input">枚举类型,字符串,区分大小写</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ListResultDto<ComboboxItemDtoT<string>>> GetEnumForCombobox(string input)
        {
            await Task.Yield();

            var items = default(IEnumerable<ComboboxItemDtoT<string>>);

            switch (input)
            {
                case nameof(CourseStateEnum):
                    items = GetEnumForCombobox<CourseStateEnum>();
                    break;
                case nameof(CourseTypeEnum):
                    items = GetEnumForCombobox<CourseTypeEnum>();
                    break;
                case nameof(CourseVideoTypeEnum):
                    items = GetEnumForCombobox<CourseVideoTypeEnum>();
                    break;

                case nameof(OrderStatusEnum):
                    items = GetEnumForCombobox<OrderStatusEnum>();
                    break;
                case nameof(OrderSourceType):
                    items = GetEnumForCombobox<OrderSourceType>();
                    break;
                case nameof(CourseClassHourResourcTypeEnum):
                    items = GetEnumForCombobox<CourseClassHourResourcTypeEnum>();
                    break;
            }

            return new ListResultDto<ComboboxItemDtoT<string>>()
            {
                Items = items?.ToList()?.AsReadOnly()
            };
        }

        private IEnumerable<ComboboxItemDtoT<string>> GetEnumForCombobox<TEnum>()
        {
            return _enumExtensionsAppService
                  .GetEntityDoubleStringKeyValueList<TEnum>()
                  .Select(o =>
                  {
                      return new ComboboxItemDtoT<string>()
                      {
                          DisplayText = o.Key,
                          Value = o.Value
                      };
                  });
        }

        //public async Task<string> CreateOrder()
        //{
        //    var product = await _productManager.GetProductByCode("000315504845040013");
        //    // 生成ordercode
        //    string tradeno = _orderManager.GetOrderMaxCode(OrderSourceType.Product, PayTypeEnum.Alipay);

        //    // 创建订单
        //    var order = new Order
        //    {
        //        OrderCode = tradeno,
        //        ProductCode = product.Code,
        //        ProductId = product.Id,
        //        ProductIndate = product.Indate,
        //        ProductCreateProjectCount = product.CreateProjectCount,
        //        ProductName = product.Name,
        //        Price = product.Price,
        //        Status = OrderStatusEnum.ChangeHands,
        //        Discounts = product.Price,
        //        ActualPayment = 0,
        //        UserId = AbpSession.UserId,
        //        UserName = AbpSession.GetUserName(),
        //        OrderSourceType = OrderSourceType.Product,
        //        CourseId = null

        //    };
        //    order = await _orderManager.CreateOrder(order);

        //    return order.OrderCode;
        //}

        //public async Task ZenSongKeCheng(string orderCode)
        //{
        //    var order = await _orderManager.GetOrderByOrderCode(orderCode);
        //    await _orderManager.UpdateOrderStateByProductType(order);

        //}
    }
}

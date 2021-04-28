using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement
{
    /// <summary>
    /// Product应用层服务的接口方法
    ///</summary>
    public interface IProductAppService : IApplicationService
    {
        /// <summary>
		/// 获取Product的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ProductListDto>> GetPaged(GetProductsInput input);


        /// <summary>
        /// 通过指定id获取ProductListDto信息
        /// </summary>
        Task<ProductListDto> GetById(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetProductForEditOutput> GetForEdit(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改Product的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateProductInput input);


        /// <summary>
        /// 删除Product信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除Product
        /// </summary>
        Task BatchDelete(List<Guid> input);


        /// <summary>
        /// 获取所有产品类型
        /// </summary>
        /// <returns></returns>
        Task<List<KeyValuePair<string, string>>> GetTypes();

        /// <summary>
        /// 获取所有产品
        /// </summary>
        /// <returns></returns>
        Task<List<KeyValuePair<string, string>>> GetProducts();

        /// <summary>
        /// 发布一个产品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task PublishProduct(NullableIdDto<Guid> input);
        /// <summary>
        /// 下架产品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UnshelveProduct(NullableIdDto<Guid> input);


        /// <summary>
        /// 买框架送课程
        /// </summary>
        Task<Order> BuyProductAndSendCoursesAsync(PurchasePayInput input);







        ///// <summary>
        ///// 导出Product为excel表
        ///// </summary>
        ///// <returns></returns>
        //Task<FileDto> GetToExcel();
    }
}

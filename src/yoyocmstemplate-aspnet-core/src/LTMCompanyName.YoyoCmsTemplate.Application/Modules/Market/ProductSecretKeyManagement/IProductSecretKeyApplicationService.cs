using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductSecretKeyManagement.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductSecretKeyManagement
{
    /// <summary>
    /// ProductSecretKey应用层服务的接口方法
    ///</summary>
    public interface IProductSecretKeyAppService : IApplicationService
    {
        /// <summary>
		/// 获取ProductSecretKey的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ProductSecretKeyListDto>> GetPaged(GetProductSecretKeysInput input);


        /// <summary>
        /// 通过指定id获取ProductSecretKeyListDto信息
        /// </summary>
        Task<ProductSecretKeyListDto> GetById(EntityDto<Guid> input);


        /// <summary>
        /// 删除ProductSecretKey信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除ProductSecretKey
        /// </summary>
        Task BatchDelete(List<Guid> input);

        /// <summary>
        /// 根据产品批量创建密钥,数量不要太多,太多了卡
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task BatchCreate(BatchCreateProductSecretKeyInput input);

        /// <summary>
        /// 绑定卡密到用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task BindToUser(ProductSecretKeyBindToUserInput input);

        ///// <summary>
        ///// 导出ProductSecretKey为excel表
        ///// </summary>
        ///// <returns></returns>
        //Task<FileDto> GetToExcel();

    }
}

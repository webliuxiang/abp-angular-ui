using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductSecretKeyManagement.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductSecretKeyManagement.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductSecretKeyManagement
{
    /// <summary>
    /// ProductSecretKey应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class ProductSecretKeyAppService : YoyoCmsTemplateAppServiceBase, IProductSecretKeyAppService
    {
        private readonly IRepository<ProductSecretKey, Guid> _entityRepository;

        private readonly IProductSecretKeyManager _entityManager;

        private readonly IProductManager _productManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ProductSecretKeyAppService(
        IRepository<ProductSecretKey, Guid> entityRepository
        , IProductSecretKeyManager entityManager,
        IProductManager productManager
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
            _productManager = productManager;
        }


        /// <summary>
        /// 获取ProductSecretKey的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[AbpAuthorize(ProductSecretKeyPermissions.Query)]
        public async Task<PagedResultDto<ProductSecretKeyListDto>> GetPaged(GetProductSecretKeysInput input)
        {

            var query = _entityRepository.GetAll()
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(),
                    o => o.ProductCode.Contains(input.FilterText)
                    || o.UserName.Contains(input.FilterText)
                    || o.OrderCode.Contains(input.FilterText)
                );
            // TODO:根据传入的参数添加过滤条件


            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting)
                    .AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            var entityListDtos = ObjectMapper.Map<List<ProductSecretKeyListDto>>(entityList);
            //var entityListDtos = entityList.MapTo<List<ProductSecretKeyListDto>>();

            return new PagedResultDto<ProductSecretKeyListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取ProductSecretKeyListDto信息
        /// </summary>
        [AbpAuthorize(ProductSecretKeyPermissions.Query)]
        public async Task<ProductSecretKeyListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return ObjectMapper.Map<ProductSecretKeyListDto>(entity);

            //return entity.MapTo<ProductSecretKeyListDto>();
        }



        /// <summary>
        /// 新增ProductSecretKey
        /// </summary>
        [AbpAuthorize(ProductSecretKeyPermissions.Create)]
        protected virtual async Task<ProductSecretKeyEditDto> Create(ProductSecretKeyEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<ProductSecretKey>(input);
            //var entity = input.MapTo<ProductSecretKey>();


            entity = await _entityRepository.InsertAsync(entity);

            return ObjectMapper.Map<ProductSecretKeyEditDto>(entity);
            //return entity.MapTo<ProductSecretKeyEditDto>();
        }

        /// <summary>
        /// 编辑ProductSecretKey
        /// </summary>
        [AbpAuthorize(ProductSecretKeyPermissions.Edit)]
        protected virtual async Task Update(ProductSecretKeyEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            //input.MapTo(entity);

            ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除ProductSecretKey信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(ProductSecretKeyPermissions.Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除ProductSecretKey的方法
        /// </summary>
        [AbpAuthorize(ProductSecretKeyPermissions.BatchDelete)]
        public async Task BatchDelete(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        [AbpAuthorize(ProductSecretKeyPermissions.Edit)]
        public async Task BindToUser(ProductSecretKeyBindToUserInput input)
        {
            await _entityManager.BindToUser(input.SecretKey, input.UserName, input.Money);
        }

        [AbpAuthorize(ProductSecretKeyPermissions.Create)]
        public async Task BatchCreate(BatchCreateProductSecretKeyInput input)
        {
            if (input.Quantity <= 0)
            {
                return;
            }

            var product = await _productManager.GetProductByCode(input.ProductCode);
            if (product == null)
            {
                throw new UserFriendlyException("未找到此产品,请刷新页面后重试");
            }

            var entityList = new List<ProductSecretKey>();
            var tmpSecretKey = string.Empty;
            for (var i = 0; i < input.Quantity; i++)
            {
                tmpSecretKey = GenSecretKey(i, input.ProductCode);
                // 校验一下
                if ((await _entityRepository.GetAll().Where(o => o.SecretKey == tmpSecretKey).CountAsync()) != 0)
                {
                    i--;
                    continue;
                }


                entityList.Add(new ProductSecretKey()
                {
                    SecretKey = tmpSecretKey,
                    ProductId = product.Id,
                    ProductCode = product.Code,
                    Used = false,
                });
            }

            foreach (var entity in entityList)
            {
                await _entityRepository.InsertAsync(entity);
            }
        }

        /// <summary>
        /// 生成产品密钥
        /// </summary>
        /// <param name="index"></param>
        /// <param name="productCode"></param>
        /// <returns></returns>
        private static string GenSecretKey(int index, string productCode)
        {



            return StringExtensions.ToMd5($"{index}{Guid.NewGuid()}{productCode}");
        }




        ///// <summary>
        ///// 导出ProductSecretKey为excel表,等待开发。
        ///// </summary>
        ///// <returns></returns>
        //public async Task<FileDto> GetToExcel()
        //{
        //	var users = await UserManager.Users.ToListAsync();
        //	var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
        //	await FillRoleNames(userListDtos);
        //	return _userListExcelExporter.ExportToFile(userListDtos);
        //}

    }
}



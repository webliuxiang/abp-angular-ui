using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using L._52ABP.Common.Extensions.Enums;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Extension;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.DownloadLogs;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.OrderManagement.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.DomainService;
using LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo;
using LTMCompanyName.YoyoCmsTemplate.Modules.Mooc.CourseInfo.DomainService;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Market.ProductManagement
{
    /// <summary>
    /// Product应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]

    public class ProductAppService : YoyoCmsTemplateAppServiceBase, IProductAppService
    {
        private readonly IRepository<Product, Guid> _entityRepository;

        private readonly IProductManager _entityManager;
        private readonly IOrderManager _orderManager;
        private readonly CourseManager _courseManager;
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IRepository<Course, long> _courseRepository;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ProductAppService(
        IRepository<Product, Guid> entityRepository
        , IProductManager entityManager, IOrderManager orderManager, CourseManager courseManager, IRepository<Course, long> courseRepository, IRepository<Order, Guid> orderRepository)
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
            _orderManager = orderManager;
            _courseManager = courseManager;
            _courseRepository = courseRepository;
            _orderRepository = orderRepository;
        }


        /// <summary>
        /// 获取Product的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(ProductPermissions.Query)]

        public async Task<PagedResultDto<ProductListDto>> GetPaged(GetProductsInput input)
        {

            var query = _entityRepository.GetAll()
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(),
                    o => o.Name.Contains(input.FilterText)
                        || o.Code.Contains(input.FilterText)
                        || o.Type.Contains(input.FilterText)
                    );
            // TODO:根据传入的参数添加过滤条件


            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            var entityListDtos = ObjectMapper.Map<List<ProductListDto>>(entityList);
            //    var entityListDtos = entityList.MapTo<List<ProductListDto>>();

            return new PagedResultDto<ProductListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取ProductListDto信息
        /// </summary>
        [AbpAuthorize(ProductPermissions.Query)]
        public async Task<ProductListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);
            return ObjectMapper.Map<ProductListDto>(entity);
            //  return entity.MapTo<ProductListDto>();
        }

        /// <summary>
        /// 获取编辑 Product
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(ProductPermissions.Create, ProductPermissions.Edit)]
        public async Task<GetProductForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetProductForEditOutput();
            ProductEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                //  editDto = entity.MapTo<ProductEditDto>();

                editDto = ObjectMapper.Map<ProductEditDto>(entity);
            }
            else
            {
                editDto = new ProductEditDto();
            }

            output.Types = await this.GetTypes();
            output.Product = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Product的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(ProductPermissions.Create, ProductPermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateProductInput input)
        {

            if (input.Product.Id.HasValue)
            {
                await Update(input.Product);
            }
            else
            {
                await Create(input.Product);
            }
        }


        /// <summary>
        /// 新增Product
        /// </summary>
        [AbpAuthorize(ProductPermissions.Create)]
        protected virtual async Task<ProductEditDto> Create(ProductEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            if (!input.CreateProjectCount.HasValue || input.CreateProjectCount.Value < 0)
            {
                throw new UserFriendlyException("项目创建次数必须大于等于0");
            }
            if (!input.Price.HasValue || input.Price.Value < 0)
            {
                throw new UserFriendlyException("产品价格必须大于等于0");
            }
            //if (!input.Indate.HasValue || input.Indate.Value < 0)
            //{
            //    throw new UserFriendlyException("有效天数必须大于等于0");
            //}




            var entity = ObjectMapper.Map<Product>(input);
            //  var entity = input.MapTo<Product>();

            // 生成产品编码
            entity.Code = ProductCodeGen.GetCode((int)entity.Type.ToEnum<DownloadTypeEnum>(true), AbpSession.UserId.Value);

            entity = await _entityRepository.InsertAsync(entity);
            return ObjectMapper.Map<ProductEditDto>(entity);
        }

        /// <summary>
        /// 编辑Product
        /// </summary>
        [AbpAuthorize(ProductPermissions.Edit)]
        protected virtual async Task Update(ProductEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            if (!input.CreateProjectCount.HasValue || input.CreateProjectCount.Value < 0)
            {
                throw new UserFriendlyException("项目创建次数必须大于等于0");
            }
            if (!input.Price.HasValue || input.Price.Value < 0)
            {
                throw new UserFriendlyException("产品价格必须大于等于0");
            }
            //if (!input.Indate.HasValue || input.Indate.Value < 0)
            //{
            //    throw new UserFriendlyException("有效天数必须大于等于0");
            //}



            var entity = await _entityRepository.GetAsync(input.Id.Value);
            if (entity.Published)
            {
                throw new UserFriendlyException("产品已经发布,禁止修改!");
            }

            // 产品编码不允许被修改,直接使用原有编码进行覆盖,防止操作dom和模拟post修改
            input.Code = entity.Code;


            //   input.MapTo(entity);


            ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除Product信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(ProductPermissions.Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            var entity = await _entityRepository.GetAsync(input.Id);
            if (entity.Published)
            {
                throw new UserFriendlyException("已发布的产品禁止删除");
            }

            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Product的方法
        /// </summary>
        [AbpAuthorize(ProductPermissions.BatchDelete)]
        public async Task BatchDelete(List<Guid> input)
        {
            var entitys = await _entityRepository.GetAll()
                .Where(o => input.Contains(o.Id))
                .ToListAsync();

            foreach (var entity in entitys)
            {
                if (entity.Published)
                {
                    throw new UserFriendlyException("存在已发布的产品,已发布的产品禁止删除");
                }
            }

            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        public async Task<List<KeyValuePair<string, string>>> GetTypes()
        {
            await Task.Yield();
            var enumExtensionsAppService = IocManager.Instance.Resolve<IEnumExtensionsAppService>();
            return enumExtensionsAppService.GetStringKeyValueList<DownloadTypeEnum>();
        }

        public async Task<List<KeyValuePair<string, string>>> GetProducts()
        {
            var allData = await _entityRepository.GetAllListAsync(o => o.Published);
            return allData.Select(o => new KeyValuePair<string, string>(o.Name, o.Code))
                .ToList();
        }
        [AbpAuthorize(ProductPermissions.Edit)]

        public async Task PublishProduct(NullableIdDto<Guid> input)
        {
            if (!input.Id.HasValue)
            {
                throw new UserFriendlyException("请输入产品编号");
            }

            var product = await _entityRepository.GetAsync(input.Id.Value);

            if (product.Published)
            {
                return;
            }

            product.Published = true;
            await _entityRepository.UpdateAsync(product);

        }
        [AbpAuthorize(ProductPermissions.Edit)]

        public async Task UnshelveProduct(NullableIdDto<Guid> input)
        {
            if (!input.Id.HasValue)
            {
                throw new UserFriendlyException("请选择要下架的产品");
            }

            var product = await _entityRepository.GetAsync(input.Id.Value);

            if (!product.Published)
            {
                return;
            }

            product.Published = false;
            await _entityRepository.UpdateAsync(product);
        }

        /// <summary>
        /// 买产品送课程
        /// </summary>
        /// <returns></returns>
        public async Task<Order> BuyProductAndSendCoursesAsync(PurchasePayInput input)
        {
            //  //检查当前产品是否已经付款过了。还需要判断有效期是否过了。暂时不做处理。
            //var PreOrder=  _orderRepository.FirstOrDefaultAsync(a => a.ProductCode == input.ProductCode&&a.Status==OrderStatusEnum.ChangeHands);

            //if (PreOrder!=null)
            //{
            //    throw  new UserFriendlyException("该产品已经付款了，");
            //}

            var order = new Order();
            if (input.OrderSourceType == OrderSourceType.Course)
            {
                order = await CreateCourseOrder(input);
            }
            else if (input.OrderSourceType == OrderSourceType.Product)
            {
                order = await CreateProductOrder(input);
            }

            return order;
        }

        ///// <summary>
        ///// 导出Product为excel表,等待开发。
        ///// </summary>
        ///// <returns></returns>
        //public async Task<FileDto> GetToExcel()
        //{
        //	var users = await UserManager.Users.ToListAsync();
        //	var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
        //	await FillRoleNames(userListDtos);
        //	return _userListExcelExporter.ExportToFile(userListDtos);
        //}

        #region 生成订单逻辑

        /// <summary>
        /// 创建课程订单
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isGiving">是否赠送</param>
        /// <returns></returns>
        private async Task<Order> CreateCourseOrder(PurchasePayInput input, bool isGiving = false)
        {
            //根据产品编码查询对应产品
            var course = await _courseRepository.FirstOrDefaultAsync(a => a.CourseCode == input.ProductCode);
            return await _orderManager.CreateCourseOrder(course);



        }

        /// <summary>
        /// 创建产品框架订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<Order> CreateProductOrder(PurchasePayInput input)
        {

            var product = await _entityManager.GetProductByCode(input.ProductCode);

            // 生成ordercode
            var tradeno = _orderManager.GetOrderMaxCode(input.OrderSourceType, PayTypeEnum.Alipay);

            // 创建订单
            var order = new Order
            {
                OrderCode = tradeno,
                ProductCode = product.Code,
                ProductId = product.Id,
                ProductIndate = product.Indate,
                ProductCreateProjectCount = product.CreateProjectCount,
                ProductName = product.Name,
                Price = product.Price,
                Status = OrderStatusEnum.WaitForPayment,
                Discounts = 0m,
                ActualPayment = product.Price,
                UserId = AbpSession.UserId,
                UserName = AbpSession.GetUserName(),
                OrderSourceType = input.OrderSourceType,
                CourseId = null

            };
            order = await _orderManager.CreateOrder(order);

            return order;
        }



        // 课程订单 //框架订单
        // 所有的课程全部免费




        #endregion



    }
}



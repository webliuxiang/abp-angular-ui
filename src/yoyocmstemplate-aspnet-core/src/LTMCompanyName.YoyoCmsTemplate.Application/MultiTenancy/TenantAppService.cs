using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Features;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using LTMCompanyName.YoyoCmsTemplate.Authorization;
using LTMCompanyName.YoyoCmsTemplate.Authorization.Roles;
using LTMCompanyName.YoyoCmsTemplate.Editions;
using LTMCompanyName.YoyoCmsTemplate.Editions.Dtos;
using LTMCompanyName.YoyoCmsTemplate.MultiTenancy.Dtos;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.MultiTenancy
{
    [AbpAuthorize(YoyoSoftPermissionNames.Pages_Tenants)]
    public class TenantAppService : YoyoCmsTemplateAppServiceBase, ITenantAppService
    {
        private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;
        private readonly EditionManager _editionManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Tenant, int> _tenantRepository;
        private readonly UserManager _userManager;

        public TenantAppService(
            IRepository<Tenant, int> tenantRepository,
            EditionManager editionManager,
            UserManager userManager,
            RoleManager roleManager,
            IAbpZeroDbMigrator abpZeroDbMigrator,
            IPasswordHasher<User> passwordHasher)
        {
            _tenantRepository = tenantRepository;
            _editionManager = editionManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _abpZeroDbMigrator = abpZeroDbMigrator;
            _passwordHasher = passwordHasher;
        }

        public async Task<PagedResultDto<TenantListDto>> GetPaged(GetTenantsInput input)
        {
            // 
            var query = TenantManager.Tenants
                .Include(o => o.Edition)
                // 过滤名称
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(),
                    o => o.Name.Contains(input.FilterText) || o.TenancyName.Contains(input.FilterText))
                // 过滤创建时间范围
                .WhereIf(input.CreationDateStart.HasValue, o => o.CreationTime >= input.CreationDateStart.Value)
                .WhereIf(input.CreationDateEnd.HasValue, o => o.CreationTime <= input.CreationDateEnd.Value)
                // 过滤订阅时间范围
                .WhereIf(input.SubscriptionStart.HasValue,
                    o => o.SubscriptionEndUtc >= input.SubscriptionStart.Value.ToUniversalTime())
                .WhereIf(input.SubscriptionEnd.HasValue,
                    o => o.SubscriptionEndUtc <= input.SubscriptionEnd.Value.ToUniversalTime())
                // 过滤版本
                .WhereIf(input.EditionId.HasValue, o => o.EditionId == input.EditionId);


            // 
            var tenantCount = await query.CountAsync();
            // 
            var tenants = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<TenantListDto>(
                tenantCount,
                ObjectMapper.Map<List<TenantListDto>>(tenants)
            );
        }


        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Tenants_Create)]
        public async Task Create(CreateTenantInput input)
        {
            await TenantManager.CreateWithAdminUser(
                input.TenancyName,
                input.Name,
                input.AdminPassword,
                input.AdminEmailAddress,
                input.ConnectionString,
                input.IsActive,
                input.EditionId,
                input.ShouldChangePasswordOnNextLogin,
                input.SendActivationEmail,
                input.SubscriptionEndUtc,
                input.IsInTrialPeriod,
                null // TODO: 这里创建激活链接
            );
        }



        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Tenants_Edit)]
        public async Task Update(TenantEditDto input)
        {
            input.ConnectionString = SimpleStringCipher.Instance.Encrypt(input.ConnectionString);
            var tenant = await TenantManager.GetByIdAsync(input.Id);
            ObjectMapper.Map(input, tenant);
            tenant.SubscriptionEndUtc = tenant.SubscriptionEndUtc?.ToUniversalTime();

            await TenantManager.UpdateAsync(tenant);
        }


        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Tenants_Edit)]
        public async Task<TenantEditDto> GetForEdit(EntityDto input)
        {
            var tenantEditDto = ObjectMapper.Map<TenantEditDto>(await TenantManager.GetByIdAsync(input.Id));
            tenantEditDto.ConnectionString = SimpleStringCipher.Instance.Decrypt(tenantEditDto.ConnectionString);
            return tenantEditDto;
        }


        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Tenants_Edit)]
        public async Task UnlockTenantAdmin(EntityDto input)
        {
            using (CurrentUnitOfWork.SetTenantId(input.Id))
            {
                var tenantAdmin = await UserManager.FindByNameAsync(AbpUserBase.AdminUserName);
                if (tenantAdmin != null) tenantAdmin.Unlock();
            }

            await Task.Delay(1);
        }


        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Tenants_Delete)]
        public async Task Delete(EntityDto input)
        {
            var tenant = await TenantManager.GetByIdAsync(input.Id);
            await TenantManager.DeleteAsync(tenant);
        }


        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Tenants_BatchDelete)]
        public async Task BatchDelete(List<EntityDto> input)
        {
            foreach (var entity in input)
            {
                var tenant = await TenantManager.GetByIdAsync(entity.Id);
                await TenantManager.DeleteAsync(tenant);
            }
        }


        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Tenants_ChangeFeatures)]
        public async Task<GetTenantFeaturesEditOutput> GetTenantFeaturesForEdit(EntityDto input)
        {
            var features = FeatureManager.GetAll()
                .Where(f => f.Scope.HasFlag(FeatureScopes.Tenant));
            var featureValues = await TenantManager.GetFeatureValuesAsync(input.Id);

            return new GetTenantFeaturesEditOutput
            {
                Features = ObjectMapper.Map<List<FlatFeatureDto>>(features).OrderBy(f => f.DisplayName).ToList(),
                FeatureValues = featureValues.Select(fv => new NameValueDto(fv)).ToList()
            };
        }

        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Tenants_ChangeFeatures)]
        public async Task UpdateTenantFeatures(UpdateTenantFeaturesInput input)
        {
            await TenantManager.SetFeatureValuesAsync(
                    input.Id,
                    input.FeatureValues.Select(fv => new NameValue(fv.Name, fv.Value)
                ).ToArray());
        }

        [AbpAuthorize(YoyoSoftPermissionNames.Pages_Tenants_ChangeFeatures)]
        public async Task ResetTenantSpecificFeatures(EntityDto input)
        {
            await TenantManager.ResetAllFeaturesAsync(input.Id);
        }
    }
}
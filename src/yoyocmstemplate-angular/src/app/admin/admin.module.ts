import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from '@app/admin/admin-routing.module';
import { UsersComponent } from '@app/admin/users/users.component';
import { LanguagesComponent } from '@app/admin/languages/languages.component';
import { OrganizationUnitsComponent } from '@app/admin/organization-units/organization-units.component';
import { RolesComponent } from '@app/admin/roles/roles.component';
import { AuditLogsComponent } from '@app/admin/audit-logs/audit-logs.component';
import { TenantsComponent } from '@app/admin/tenants/tenants.component';
import { MaintenanceComponent } from '@app/admin/maintenance/maintenance.component';
import { EditionsComponent } from '@app/admin/editions/editions.component';
import { HostSettingsComponent } from '@app/admin/host-settings/host-settings.component';
import { TenantSettingsComponent } from '@app/admin/tenant-settings/tenant-settings.component';
import { SubscriptionManagementComponent } from '@app/admin/subscription-management/subscription-management.component';
import { LanguageTextsComponent } from '@app/admin/language-texts/language-texts.component';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from '@shared/shared.module';
import { AbpModule } from 'abp-ng2-module';
import { CreateOrEditUserComponent } from '@app/admin/users/create-or-edit-user/create-or-edit-user.component';
import { EditUserPermissionsComponent } from '@app/admin/users/edit-user-permissions/edit-user-permissions.component';
import { PermissionTreeComponent } from '@app/admin/shared/permission-tree/permission-tree.component';
import { CreateOrEditRoleComponent } from '@app/admin/roles/create-or-edit-role/create-or-edit-role.component';
import { AuditLogsDetailComponent } from '@app/admin/audit-logs/audit-logs-detail/audit-logs-detail.component';
// tslint:disable-next-line:max-line-length
import { AddMemberComponent } from '@app/admin/organization-units/add-member/add-member.component';
import { CreateOrEditLanguageComponent } from '@app/admin/languages/create-or-edit-language/create-or-edit-language.component';
import { EditLanguageTextComponent } from '@app/admin/language-texts/edit-language-text/edit-language-text.component';
import { CreateTenantComponent } from '@app/admin/tenants/create-tenant/create-tenant.component';
import { EditTenantComponent } from '@app/admin/tenants/edit-tenant/edit-tenant.component';
// tslint:disable-next-line:max-line-length
import { CreateOrEditOrganiaztionUnitComponent } from '@app/admin/organization-units/create-or-edit-organiaztion-unit/create-or-edit-organiaztion-unit.component';
import { PermissionComboxComponent } from '@app/admin/shared/permission-combox/permission-combox.component';
import { RoleComboxComponent } from '@app/admin/shared/role-combox/role-combox.component';
// tslint:disable-next-line:max-line-length
import { OrganizationUnitTreePanelComponent } from '@app/admin/organization-units/organization-unit-tree-panel/organization-unit-tree-panel.component';
// tslint:disable-next-line:max-line-length
import { OrganizationUnitMembersPanelComponent } from '@app/admin/organization-units/organization-unit-members-panel/organization-unit-members-panel.component';
import { OrganizationUnitsTreeComponent } from '@app/admin/shared/organization-unit-tree/organization-unit-tree.component';
import { FormsModule } from '@angular/forms';
import { EditionComboComponent } from './shared/edition-combo/edition-combo.component';
import { CreateOrEditEditionComponent } from './editions/create-or-edit-edition/create-or-edit-edition.component';
import { TimeZoneComboComponent } from './shared/timing/timezone-combo.component';
import { EditionFeatureTreeComponent } from './shared/edition-feature-tree/edition-feature-tree.component';
import { EditTenantFeaturesComponent } from './tenants/edit-tenant-features/edit-tenant-features.component';
import { CommonLookupComponent } from './common/common-lookup/common-lookup.component';

import { OrganizationUnitRolePanelComponent } from './organization-units/organization-unit-role-panel/organization-unit-role-panel.component';
import { AddRoleComponent } from './organization-units/add-role/add-role.component';
import { SysFileManagerComponent } from './file-manager/file-manager.component';

@NgModule({
  imports: [FormsModule, CommonModule, HttpClientModule, SharedModule, AbpModule, AdminRoutingModule],
  declarations: [
    UsersComponent,
    LanguagesComponent,
    OrganizationUnitsComponent,
    RolesComponent,
    SysFileManagerComponent,
    AuditLogsComponent,
    TenantsComponent,
    MaintenanceComponent,
    EditionsComponent,
    HostSettingsComponent,
    TenantSettingsComponent,
    SubscriptionManagementComponent,
    LanguageTextsComponent,
    PermissionComboxComponent, // 权限树形列表
    RoleComboxComponent, // 角色树形列表
    CreateOrEditUserComponent,
    OrganizationUnitsTreeComponent, // 组织单元树展示
    EditUserPermissionsComponent,
    PermissionTreeComponent,
    CreateOrEditRoleComponent,
    AuditLogsDetailComponent,
    CreateOrEditOrganiaztionUnitComponent,
    AddMemberComponent,
    CreateOrEditLanguageComponent,
    EditLanguageTextComponent,
    CreateTenantComponent,
    OrganizationUnitTreePanelComponent,
    OrganizationUnitMembersPanelComponent,
    EditTenantComponent,
    EditionComboComponent,
    CreateOrEditEditionComponent,
    TimeZoneComboComponent,
    EditionFeatureTreeComponent,
    EditTenantFeaturesComponent,
    CommonLookupComponent,

    OrganizationUnitRolePanelComponent,
    AddRoleComponent,
  ],
  entryComponents: [
    CreateOrEditUserComponent,
    EditUserPermissionsComponent,
    CreateOrEditRoleComponent,
    AuditLogsDetailComponent,
    CreateOrEditOrganiaztionUnitComponent,
    AddMemberComponent,
    AddRoleComponent,
    CreateOrEditLanguageComponent,
    EditLanguageTextComponent,
    CreateTenantComponent,
    EditTenantComponent,
    CreateOrEditEditionComponent,
    EditTenantFeaturesComponent,
    CommonLookupComponent,
  ],
  providers: [],
})
export class AdminModule {}

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UsersComponent } from '@app/admin/users/users.component';
import { RolesComponent } from '@app/admin/roles/roles.component';
import { AuditLogsComponent } from '@app/admin/audit-logs/audit-logs.component';
import { MaintenanceComponent } from '@app/admin/maintenance/maintenance.component';
import { HostSettingsComponent } from '@app/admin/host-settings/host-settings.component';
import { EditionsComponent } from '@app/admin/editions/editions.component';
import { LanguagesComponent } from '@app/admin/languages/languages.component';
import { LanguageTextsComponent } from '@app/admin/language-texts/language-texts.component';
import { TenantsComponent } from '@app/admin/tenants/tenants.component';
import { OrganizationUnitsComponent } from '@app/admin/organization-units/organization-units.component';
import { SubscriptionManagementComponent } from '@app/admin/subscription-management/subscription-management.component';
import { TenantSettingsComponent } from '@app/admin/tenant-settings/tenant-settings.component';
import { SysFileManagerComponent } from './file-manager/file-manager.component';

const routes: Routes = [
  { path: 'users', component: UsersComponent },
  { path: 'roles', component: RolesComponent },
  { path: 'auditLogs', component: AuditLogsComponent },
  { path: 'maintenance', component: MaintenanceComponent },
  { path: 'host-settings', component: HostSettingsComponent },
  { path: 'editions', component: EditionsComponent },
  { path: 'file-manager', component: SysFileManagerComponent },

  { path: 'languages', component: LanguagesComponent },
  { path: 'languagetexts', component: LanguageTextsComponent },
  { path: 'tenants', component: TenantsComponent },
  { path: 'organization-units', component: OrganizationUnitsComponent },
  {
    path: 'subscription-management',
    component: SubscriptionManagementComponent,
  },
  { path: 'tenant-settings', component: TenantSettingsComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}

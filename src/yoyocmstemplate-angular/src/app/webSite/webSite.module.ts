import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from '@shared/shared.module';
import { AbpModule } from 'abp-ng2-module';
import { WebSiteRoutingModule } from './webSite-routing.module';
import { BlogrollTypeComponent } from './blogrolls/blogroll-type.component';
import { CreateOrEditBlogrollTypeComponent } from './blogrolls/create-or-edit-blogroll-type/create-or-edit-blogroll-type.component';
import { BlogrollComponent } from './blogrolls/blogroll.component';
import { CreateOrEditBlogrollComponent } from './blogrolls/create-or-edit-blogroll/create-or-edit-blogroll.component';
import { WebSiteNoticeComponent } from './notices/web-site-notice.component';
import { CreateOrEditWebSiteNoticeComponent } from './notices/create-or-edit-web-site-notice/create-or-edit-web-site-notice.component';
import { BannerAdComponent } from './banner-ads/banner-ad.component';
import { CreateOrEditBannerAdComponent } from './banner-ads/create-or-edit-banner-ad/create-or-edit-banner-ad.component';

@NgModule({
  imports: [CommonModule, HttpClientModule, SharedModule, AbpModule, WebSiteRoutingModule],

  declarations: [
    BannerAdComponent,
    CreateOrEditBannerAdComponent,
    BlogrollTypeComponent,
    CreateOrEditBlogrollTypeComponent,
    BlogrollComponent,
    CreateOrEditBlogrollComponent,

    WebSiteNoticeComponent,
    CreateOrEditWebSiteNoticeComponent,
  ],
  entryComponents: [
    CreateOrEditBannerAdComponent,
    CreateOrEditBlogrollTypeComponent,
    CreateOrEditBlogrollComponent,
    CreateOrEditWebSiteNoticeComponent,
  ],
})
export class WebSiteModule {}

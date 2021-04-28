import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BlogrollTypeComponent } from './blogrolls/blogroll-type.component';
import { BlogrollComponent } from './blogrolls/blogroll.component';
import { WebSiteNoticeComponent } from './notices/web-site-notice.component';
import { BannerAdComponent } from './banner-ads/banner-ad.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: 'bannerads', component: BannerAdComponent, data: { permission: 'Pages.BannerAd' } },
      { path: 'blogrollstype', component: BlogrollTypeComponent, data: { permission: 'Pages.BlogrollType' } },
      { path: 'blogrolls', component: BlogrollComponent, data: { permission: 'Pages.Blogroll' } },
      { path: 'notices', component: WebSiteNoticeComponent, data: { permission: 'Pages.WebSiteNotice' } },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WebSiteRoutingModule {}

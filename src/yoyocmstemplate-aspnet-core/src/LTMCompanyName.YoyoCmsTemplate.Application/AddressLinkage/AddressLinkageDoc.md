# 省市县镇组件

#### 前端组件说明

- **组件路径 **
52abp-pro\src\yoyocmstemplate-angular\src\app\main\x-address-linkage

- 使用示例
      <app-x-address-linkage>
	  [name]="addressLinkage"
	  [placeHolder]="请选择"
	  [xDisabled]="false"
	  [isClickOnLoad]="true"
      (selectNameChange)="getSelectName($event)"
	  (selectionChange)="getChangeCode($event)">
	  </app-x-address-linkage>
    placeHolder ：输出框无值展示的内容
	xDisabled : 是否禁用控件
	isClickOnLoad : 加载事件  ture 点击加载 ；false：鼠标移动加载
	selectNameChange : 选择事件，返回名称数组
	selectionChange: 选择事件，返回编码数组


#### 后端Api说明
- **后端Api路径 **
52abp-pro\src\yoyocmstemplate-aspnet-core\src\LTMCompanyName.YoyoCmsTemplate.Application\AddressLinkage
- **数据源保存路径 **
52abp-pro\src\yoyocmstemplate-aspnet-core\src\LTMCompanyName.YoyoCmsTemplate.Web.Host\wwwroot\AddressLinkage
- **后端Api说明 **
1. /api/services/app/AddressLinkage/Get          获取省市区县镇数据
2. /api/services/app/AddressLinkage/GetAll       获取所有数据
3. /api/services/app/AddressLinkage/GetByCode    通过code获取省市区县镇名称（没有code传空）
4. /api/services/app/AddressLinkage/GetAllCity   <abbr title="暂无使用">获取所有市数据</abbr>
5. /api/services/app/AddressLinkage/GetAllArea   <abbr title="暂无使用">获取所有县数据</abbr>
6. /api/services/app/AddressLinkage/GetAllStreet <abbr title="暂无使用">获取所有镇数据</abbr>






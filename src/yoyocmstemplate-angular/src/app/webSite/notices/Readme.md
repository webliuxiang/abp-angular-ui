

# 52ABP-Pro Angular前端配置说明 

 
## 解析服务代理service-proxy.module 配置

1. 首先在前端项目如(52abpPro-angular)的根目录中运行路径为 `nswag-> refresh.bat` 的批处理文件，以更新所有的后端服务接口。

2. 打开文件夹路径为`\src\shared\service-proxies\`中的service-proxy.module.ts文件

2. 打开**service-proxy.module.ts**文件，添加以下代码：

```
@NgModule({
	providers: [
			//以下内容复制进去
			ApiServiceProxies.WebSiteNoticeServiceProxy,
			//
			],
	})

```
## 左侧菜单栏的配置说明 

> 从52abp-pro的3.1.2的版本中，我们添加了排序功能，
> 您可以继续采用以前的方式添加菜单，我们建议采用现在的新方法，在新的AppMainMenus文件中添加。
> 而旧的AppMenus.ts用于已经维护好的管理端的模块功能

打开文件夹路径为`\src\abpPro\`中的`AppMainMenus.ts`文件。

在`AppMenus.ts`文件中添加以下代码作为菜单路径,复制以下最新代码段内容：
 
```ts
{//	网站公告的菜单按钮
text:'WebSiteNotice',i18n:'WebSiteNotice',acl:'Pages.WebSiteNotice',icon:'iconfont icon-dashboard',link:'/app/main/notices',sort:99},

```

请注意：以上link的值 '/app/main/notices') 中的 main在是指模块名称为main.module.ts，所以在添加菜单时请注意。
当然你可以自行更改到你的特定模块下,如要修改到admin.module.ts模块中，修改为/app/admin即可。


> 如果您使用的是低版本模板`yoyo-ng-module`的话，请复制以下代码段内容：

```

new MenuItem('WebSiteNotice', 'Pages.WebSiteNotice', 'iconfont icon-dashboard', '/app/main/notices')

```

## 路由配置说明

推荐默认的菜单配置到main模块中，
所以需要配置对应的main路由即文件`main-routing.module.ts`


打开路径为`src\app\main\main-routing.module.ts`
选择以下代码段,二选一
 //包含权限验证 
 ```
{  path: 'notices', component:WebSiteNoticeComponent,data: { permission: 'Pages.WebSiteNotice' }  }, 

```
或
//不带权限验证
```
{  path: 'notices', component:WebSiteNoticeComponent }, 
```

以上二选一,复制粘贴到路由模块中的 children: [] 数组中

 
## 配置模块代码 

和路由的配置同样的道理，我们需要配置服务到模块中。
打开路径`src\app\main\main.module.ts` 的文件

添加以下代码到使用的 @NgModule 中的代码
### ================ 在 declarations 项中:

```
WebSiteNoticeComponent,
CreateOrEditWebSiteNoticeComponent,

```

### ================ 在 entryComponents 项中:

```
CreateOrEditWebSiteNoticeComponent,
```
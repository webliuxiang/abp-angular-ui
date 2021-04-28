
# 代码生成器(ABP Code Power Tools )使用说明文档

**52ABP官方网站：[http://www.52abp.com](http://www.52abp.com)**

>欢迎您使用 ABP Code Power Tools ，.NET Core 版本。
开发代码生成器的初衷是为了让大家专注于业务开发，
而基础设施的地方，由代码生成器实现，节约大家的实现。
实现提高效率、共赢的局面。

欢迎到：[Github地址](https://github.com/52ABP/52ABP.CodeGenerator) 提供您的脑洞，
如果合理的功能我会实现哦~

## 前端说明:

- 前端代码生成的文件文件路径请在YoyoCmsTemplate.Application类库中生成的对应实体代码文件中的Client中。
如Book实体生成的文件夹地址为：D:\\52ABP-PRO\PhoneBooks.Application\Books\Client\NGZorro\中
打开文件夹中的Readme文件即可。


### 配置权限功能

如果你选择了**生成权限功能**，请打开YoyoCmsTemplate.Application类库中的YoyoCmsTemplateApplicationModule.cs类文件。
然后复制以下代码到 的PreInitialize 方法中:

```csharp
Configuration.Authorization.Providers.Add<CourseCategoryAuthorizationProvider>();

```


### 配置AbpAutoMapper
 

请打开YoyoCmsTemplate.Application类库中YoyoCmsTemplateApplicationModule.cs中的 PreInitialize 方法中:

```csharp
// 自定义AutoMapper类型映射
// 如果没有这一段就把本所有代码复制上去
Configuration.Modules.AbpAutoMapper().Configurators.Add(configuration =>
{
    // ....其他代码

    // 只需要复制这一段
CourseCategoryDtoAutoMapper.CreateMappings(configuration);

    // ....其他代码
});

```
### EntityFrameworkCore功能配置

打开EntityFrameworkCore类库在 **YoyoCmsTemplateDbContext**类文件中添加以下代码段：

```csharp
public DbSet<CourseCategory>  CourseCategorys { get; set; }

 ```
以实现将实体配置到数据库上下文中。
 
【此步骤选填】如果要使用 EntityFrameworkCore 中的 Fluent API 进行具有最高优先级的配置实体，可添加以下代码到方法```OnModelCreating```中

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
   modelBuilder.ApplyConfiguration(new CourseCategoryCfg());
 }

```

#### 添加迁移记录

如果该实体的属性值未发生改变可以跳过当前小节

打开**程序包管理器控制台**工具，指定默认项目类库以**EntityFrameworkCore**结尾，然后执行以下命令:

添加一条迁移记录

```
Add-Migration AddNewCourseCategoryEntity_Migration
```

同步实体文件到数据库中
```
Update-Database
```

接下来配置好多语言功能，然后运行项目后，即可前往前端项目的配置

## 多语言的配置

生成的多语言内容
在YoyoCmsTemplate.Core类库中对应实体下的`douyuyan.md`文件中。

## 实体渲染

实体所在文件夹名称

coursecategoryinfo

CourseCategoryInfo

## 路线图

 目前优先完成SPA 以angular 为主，
如果你有想法我替你实现前端生成的代码块。
那么请到github 贴出你的代码段。
我感兴趣的话，会配合你的。

[https://github.com/52ABP/52ABP.CodeGenerator](https://github.com/52ABP/52ABP.CodeGenerator) 提供您的脑洞，

已完成：
- [x]SPA版本的前端

待办：
- [x]暂时搞不定注释，后期想办法，通过配置文件的形式补足
- [ ]菜单栏问题，如果是MPA版本
- [ ]MPA版本的前端
## 推荐内容

52ABP官方网站：[http://www.52abp.com](http://www.52abp.com)

代码生成器帮助文档：[http://www.cnblogs.com/wer-ltm/p/8445682.html](http://www.cnblogs.com/wer-ltm/p/8445682.html)


【ASP.NetCore Mvc EF入门学习】:850720634(免费)
[![52ABP .NET CORE 实战群](http://pub.idqqimg.com/wpa/images/group.png)](https://jq.qq.com/?_wv=1027&k=5GbjOD9) 

【52ABP .NET CORE 实战群】：633751348 (免费)
[![52ABP .NET CORE 实战群](http://pub.idqqimg.com/wpa/images/group.png)](https://jq.qq.com/?_wv=1027&k=5pWtBvu)

【ABP代码生成器交流群】：104390185（收费）
[![52ABP .NET CORE 实战群](http://pub.idqqimg.com/wpa/images/group.png)](http://shang.qq.com/wpa/qunwpa?idkey=3f301fa3101d3201c391aba77803b523fcc53e59d0c68e6eeb9a79336c366d92)


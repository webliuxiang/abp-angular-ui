
#代码生成器(ABP Code Power Tools )使用说明文档

**52ABP官方网站：[http://www.52abp.com](http://www.52abp.com)**

>欢迎您使用 ABP Code Power Tools ，.net core 版本。
开发代码生成器的初衷是为了让大家专注于业务开发，
而基础设施的地方，由代码生成器实现，节约大家的实现。
实现提高效率、共赢的局面。

欢迎到：[Github地址](https://github.com/52ABP/52ABP.CodeGenerator) 提供您的脑洞，
如果合理的我会实现哦~

# 使用说明:

**配置Automapper** :

复制以下代码到Application层下的：ABPCommunityApplicationModule.cs
中的 PreInitialize 方法中:

```
// 自定义类型映射
// 如果没有这一段就把这一段复制上去
Configuration.Modules.AbpAutoMapper().Configurators.Add(configuration =>
{
    // ....

    // 只需要复制这一段
CourseMapper.CreateMappings(configuration);

    // ....
});

```

**配置权限功能**  :

如果你生成了**权限功能**。复制以下代码到 ABPCommunityApplicationModule.cs
中的 PreInitialize 方法中:

```
Configuration.Authorization.Providers.Add<CourseAuthorizationProvider>();

```

**EntityFramework功能配置**:

可以在```DbContext```中增加：

 ```
public DbSet<Course>  Courses { get; set; }

 ```

在方法```OnModelCreating```中添加

```
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CourseCfg());
        }

```


**多语言配置**  

.Core 层下 Localization->SourceFiles 中

```

<text name="Name"  value="名称"></text>
<text name="Title"  value="课程标题"></text>
<text name="Price"  value="价格"></text>
<text name="Type"  value="课程显示状态"></text>
<text name="CourseVideoType"  value="课程类型"></text>
<text name="CourseState"  value="课程状态"></text>
<text name="ImgUrl"  value="封面"></text>
<text name="Description"  value="课程简介"></text>
<text name="ValidDays"  value="有效天数,0为永久有效"></text>
<text name="StartTime"  value="直播开始时间"></text>
<text name="Period"  value="课时"></text>


<text name="Course" value="课程"></text><text name="QueryCourse"  value="查询课程"></text><text name="CreateCourse"  value="添加课程"></text><text name="EditCourse"  value="编辑课程"></text><text name="DeleteCourse"  value="删除课程"></text><text name="BatchDeleteCourse" value="批量删除课程"></text><text name="ExportCourse"  value="导出课程"></text>                             

```




 **路线图**

todo: 目前优先完成SPA 以angular 为主，
如果你有想法我替你实现前端生成的代码块。
那么请到github 贴出你的代码段。
我感兴趣的话，会配合你的。

[https://github.com/52ABP/52ABP.CodeGenerator](https://github.com/52ABP/52ABP.CodeGenerator) 提供您的脑洞，

已完成：
- [x ]SPA版本的前端

待办：
- [ ]暂时搞不定注释，后期想办法
- [ ]菜单栏问题，如果是MPA版本
- [ ]MPA版本的前端
## 广告

52ABP官方网站：[http://www.52abp.com](http://www.52abp.com)

代码生成器帮助文档：[http://www.cnblogs.com/wer-ltm/p/8445682.html](http://www.cnblogs.com/wer-ltm/p/8445682.html)

【ABP代码生成器交流群】：104390185（收费）
[![52ABP .NET CORE 实战群](http://pub.idqqimg.com/wpa/images/group.png)](http://shang.qq.com/wpa/qunwpa?idkey=3f301fa3101d3201c391aba77803b523fcc53e59d0c68e6eeb9a79336c366d92)

【52ABP .NET CORE 实战群】：633751348 (免费)
[![52ABP .NET CORE 实战群](http://pub.idqqimg.com/wpa/images/group.png)](https://jq.qq.com/?_wv=1027&k=5pWtBvu)

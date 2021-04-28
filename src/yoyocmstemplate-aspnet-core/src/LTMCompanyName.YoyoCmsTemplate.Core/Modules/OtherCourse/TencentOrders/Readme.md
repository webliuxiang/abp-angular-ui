
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
TencentOrderInfoMapper.CreateMappings(configuration);

    // ....
});

```

**配置权限功能**  :

如果你生成了**权限功能**。复制以下代码到 ABPCommunityApplicationModule.cs
中的 PreInitialize 方法中:

```
Configuration.Authorization.Providers.Add<TencentOrderInfoAuthorizationProvider>();

```

**EntityFramework功能配置**:

可以在```DbContext```中增加：

 ```
public DbSet<TencentOrderInfo>  TencentOrderInfos { get; set; }

 ```

在方法```OnModelCreating```中添加

```
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TencentOrderInfoCfg());
        }

```


**多语言配置**  

.Core 层下 Localization->SourceFiles 中

```

<text name="OrganizationId"  value="机构id"></text>
<text name="OrderNumber"  value="订单号"></text>
<text name="CourseTitle"  value="课程名称"></text>
<text name="CourseType"  value="课程类型"></text>
<text name="CreationTime"  value="创建时间"></text>
<text name="PurchaseTime"  value="支付时间"></text>
<text name="UIn"  value="购买Uin码"></text>
<text name="NickName"  value="用户昵称"></text>
<text name="TradingStatus"  value="交易状态"></text>
<text name="MobileNumber"  value="联系人电话"></text>
<text name="RecipientPhone"  value="收件人电话"></text>
<text name="RecipientName"  value="收件人姓名"></text>
<text name="RecipientAddress"  value="收件人地址"></text>
<text name="TradeType"  value="交易类型"></text>
<text name="UsersPay"  value="用户支付"></text>
<text name="PlatformSubsidies"  value="平台补贴"></text>
<text name="RefundAmount"  value="退款金额"></text>
<text name="AmountPaid"  value="实算金额"></text>
<text name="SettlementAmount"  value="结算金额"></text>
<text name="SettlementRatio"  value="结算比例"></text>
<text name="ChannelFee"  value="渠道费用"></text>
<text name="TrafficConversionFee"  value="流量转化服务费"></text>
<text name="AppleShares"  value="苹果分成"></text>
<text name="DistributorShare"  value="分销商分成"></text>
<text name="SettlementStatus"  value="结算状态"></text>
<text name="IsIosOrder"  value="是否IOS订单"></text>
<text name="IsDistributorOrder"  value="是否分销订单"></text>
<text name="OrderType"  value="订单类型"></text>
<text name="RemarkOne"  value="备注1"></text>
<text name="RemarkTwo"  value="备注2"></text>
<text name="RemarkThree"  value="备注3"></text>
<text name="IsQqGroupChecked"  value="是否加入qq群"></text>
<text name="IsGiteeChecked"  value="码云是否通过"></text>
<text name="Is52AbpChecked"  value="是否同步52abp"></text>
<text name="Platform"  value="平台"></text>


<text name="TencentOrderInfo" value="腾讯订单信息"></text><text name="QueryTencentOrderInfo"  value="查询腾讯订单信息"></text><text name="CreateTencentOrderInfo"  value="添加腾讯订单信息"></text><text name="EditTencentOrderInfo"  value="编辑腾讯订单信息"></text><text name="DeleteTencentOrderInfo"  value="删除腾讯订单信息"></text><text name="BatchDeleteTencentOrderInfo" value="批量删除腾讯订单信息"></text><text name="ExportTencentOrderInfo"  value="导出腾讯订单信息"></text>                             

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

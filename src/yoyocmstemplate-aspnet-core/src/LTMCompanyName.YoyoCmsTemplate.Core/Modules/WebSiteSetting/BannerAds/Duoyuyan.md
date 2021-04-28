

# 配置52ABP-PRO的多语言
 
 
**请注意：**
- 从52ABP-PRO 2.5.0的版本开始默认采用json配置多语言
- 属性名和字段不能重复否则框架会验证失败，需要你删除重复的键名

## Json的配置方法如下

在YoyoCmsTemplate.Core类库中，找到路径为 Localization->SourceFiles->Json文件夹下的对应文件

### 中文本地化的内容Chinese localized content

找到Json文件夹下的YoyoCmsTemplate-zh-Hans.json文件，复制以下代码进入即可。

> 请注意防止键名重复，产生异常

```json
                    //轮播图广告的多语言配置==>中文
                    
                     "BannerAdTitle": "标题",
                    "BannerAdTitleInputDesc": "请输入标题",
                     
                    
                     "BannerAdImageUrl": "图片地址",
                    "BannerAdImageUrlInputDesc": "请输入图片地址",
                     
                    
                     "BannerAdThumbImgUrl": "缩略图地址",
                    "BannerAdThumbImgUrlInputDesc": "请输入缩略图地址",
                     
                    
                     "BannerAdDescription": "描述",
                    "BannerAdDescriptionInputDesc": "请输入描述",
                     
                    
                     "BannerAdUrl": "Url",
                    "BannerAdUrlInputDesc": "请输入Url",
                     
"Weight": "权重",
"Price": "Price",
                    
                     "BannerAdTypes": "Types",
                    "BannerAdTypesInputDesc": "请输入Types",
                     
"ViewCount": "ViewCount",
					                     
                    "BannerAd": "轮播图广告",
                    "ManageBannerAd": "管理轮播图广告",
                    "QueryBannerAd": "查询轮播图广告",
                    "CreateBannerAd": "添加轮播图广告",
                    "EditBannerAd": "编辑轮播图广告",
                    "DeleteBannerAd": "删除轮播图广告",
                    "BatchDeleteBannerAd": "批量删除轮播图广告",
                    "ExportBannerAd": "导出轮播图广告",
                    "BannerAdList": "轮播图广告列表",
                     //轮播图广告的多语言配置==End
                    


```


### 英语本地化的内容English localized content
找到Json文件夹下的YoyoCmsTemplate.json文件，复制以下代码进入即可。
```json
             //轮播图广告的多语言配置==>英文
                    
                     "BannerAdTitle": "BannerAdTitle",
                    "BannerAdTitleInputDesc": "Please input your BannerAdTitle",
                     
                    
                     "BannerAdImageUrl": "BannerAdImageUrl",
                    "BannerAdImageUrlInputDesc": "Please input your BannerAdImageUrl",
                     
                    
                     "BannerAdThumbImgUrl": "BannerAdThumbImgUrl",
                    "BannerAdThumbImgUrlInputDesc": "Please input your BannerAdThumbImgUrl",
                     
                    
                     "BannerAdDescription": "BannerAdDescription",
                    "BannerAdDescriptionInputDesc": "Please input your BannerAdDescription",
                     
                    
                     "BannerAdUrl": "BannerAdUrl",
                    "BannerAdUrlInputDesc": "Please input your BannerAdUrl",
                     
"Weight": "Weight",
"Price": "Price",
                    
                     "BannerAdTypes": "BannerAdTypes",
                    "BannerAdTypesInputDesc": "Please input your BannerAdTypes",
                     
"ViewCount": "ViewCount",
					                     
                    "BannerAd": "BannerAd",
                    "ManageBannerAd": "ManageBannerAd",
                    "QueryBannerAd": "QueryBannerAd",
                    "CreateBannerAd": "CreateBannerAd",
                    "EditBannerAd": "EditBannerAd",
                    "DeleteBannerAd": "DeleteBannerAd",
                    "BatchDeleteBannerAd": "BatchDeleteBannerAd",
                    "ExportBannerAd": "ExportBannerAd",
                    "BannerAdList": "BannerAdList",
                     //轮播图广告的多语言配置==End
                    




```


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
                    //网站公告的多语言配置==>中文
                    
                     "WebSiteNoticeTitle": "标题",
                    "WebSiteNoticeTitleInputDesc": "请输入标题",
                     
                    
                     "WebSiteNoticeContent": "内容",
                    "WebSiteNoticeContentInputDesc": "请输入内容",
                     
"ViewCount": "ViewCount",
					                     
                    "WebSiteNotice": "网站公告",
                    "ManageWebSiteNotice": "管理网站公告",
                    "QueryWebSiteNotice": "查询网站公告",
                    "CreateWebSiteNotice": "添加网站公告",
                    "EditWebSiteNotice": "编辑网站公告",
                    "DeleteWebSiteNotice": "删除网站公告",
                    "BatchDeleteWebSiteNotice": "批量删除网站公告",
                    "ExportWebSiteNotice": "导出网站公告",
                    "WebSiteNoticeList": "网站公告列表",
                     //网站公告的多语言配置==End
                    


```


### 英语本地化的内容English localized content
找到Json文件夹下的YoyoCmsTemplate.json文件，复制以下代码进入即可。
```json
             //网站公告的多语言配置==>英文
                    
                     "WebSiteNoticeTitle": "WebSiteNoticeTitle",
                    "WebSiteNoticeTitleInputDesc": "Please input your WebSiteNoticeTitle",
                     
                    
                     "WebSiteNoticeContent": "WebSiteNoticeContent",
                    "WebSiteNoticeContentInputDesc": "Please input your WebSiteNoticeContent",
                     
"ViewCount": "ViewCount",
					                     
                    "WebSiteNotice": "WebSiteNotice",
                    "ManageWebSiteNotice": "ManageWebSiteNotice",
                    "QueryWebSiteNotice": "QueryWebSiteNotice",
                    "CreateWebSiteNotice": "CreateWebSiteNotice",
                    "EditWebSiteNotice": "EditWebSiteNotice",
                    "DeleteWebSiteNotice": "DeleteWebSiteNotice",
                    "BatchDeleteWebSiteNotice": "BatchDeleteWebSiteNotice",
                    "ExportWebSiteNotice": "ExportWebSiteNotice",
                    "WebSiteNoticeList": "WebSiteNoticeList",
                     //网站公告的多语言配置==End
                    




```
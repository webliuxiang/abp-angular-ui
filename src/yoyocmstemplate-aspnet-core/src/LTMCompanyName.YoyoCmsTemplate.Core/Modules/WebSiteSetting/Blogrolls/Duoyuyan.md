

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
                    //友情链接分类的多语言配置==>中文
                    
                     "BlogrollTypeName": "分类名称",
                    "BlogrollTypeNameInputDesc": "请输入分类名称",
                     
					                     
                    "BlogrollType": "友情链接分类",
                    "ManageBlogrollType": "管理友情链接分类",
                    "QueryBlogrollType": "查询友情链接分类",
                    "CreateBlogrollType": "添加友情链接分类",
                    "EditBlogrollType": "编辑友情链接分类",
                    "DeleteBlogrollType": "删除友情链接分类",
                    "BatchDeleteBlogrollType": "批量删除友情链接分类",
                    "ExportBlogrollType": "导出友情链接分类",
                    "BlogrollTypeList": "友情链接分类列表",
                     //友情链接分类的多语言配置==End
                    


```


### 英语本地化的内容English localized content
找到Json文件夹下的YoyoCmsTemplate.json文件，复制以下代码进入即可。
```json
             //友情链接分类的多语言配置==>英文
                    
                     "BlogrollTypeName": "BlogrollTypeName",
                    "BlogrollTypeNameInputDesc": "Please input your BlogrollTypeName",
                     
					                     
                    "BlogrollType": "BlogrollType",
                    "ManageBlogrollType": "ManageBlogrollType",
                    "QueryBlogrollType": "QueryBlogrollType",
                    "CreateBlogrollType": "CreateBlogrollType",
                    "EditBlogrollType": "EditBlogrollType",
                    "DeleteBlogrollType": "DeleteBlogrollType",
                    "BatchDeleteBlogrollType": "BatchDeleteBlogrollType",
                    "ExportBlogrollType": "ExportBlogrollType",
                    "BlogrollTypeList": "BlogrollTypeList",
                     //友情链接分类的多语言配置==End
                    




```
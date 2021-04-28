

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
                    //课程分类的多语言配置==>中文
                    
                     "CourseCategoryName": "名称",
                    "CourseCategoryNameInputDesc": "请输入名称",
                     
                    
                     "CourseCategoryCode": "编码",
                    "CourseCategoryCodeInputDesc": "请输入编码",
                     
                    
                     "CourseCategoryImgUrl": "图片路径",
                    "CourseCategoryImgUrlInputDesc": "请输入图片路径",
                     
"ParentId": "父类Id",
"Children": "子项",
					                     
                    "CourseCategory": "课程分类",
                    "ManageCourseCategory": "管理课程分类",
                    "QueryCourseCategory": "查询课程分类",
                    "CreateCourseCategory": "添加课程分类",
                    "EditCourseCategory": "编辑课程分类",
                    "DeleteCourseCategory": "删除课程分类",
                    "BatchDeleteCourseCategory": "批量删除课程分类",
                    "ExportCourseCategory": "导出课程分类",
                    "CourseCategoryList": "课程分类列表",
                     //课程分类的多语言配置==End
                    


```


### 英语本地化的内容English localized content
找到Json文件夹下的YoyoCmsTemplate.json文件，复制以下代码进入即可。
```json
             //课程分类的多语言配置==>英文
                    
                     "CourseCategoryName": "CourseCategoryName",
                    "CourseCategoryNameInputDesc": "Please input your CourseCategoryName",
                     
                    
                     "CourseCategoryCode": "CourseCategoryCode",
                    "CourseCategoryCodeInputDesc": "Please input your CourseCategoryCode",
                     
                    
                     "CourseCategoryImgUrl": "CourseCategoryImgUrl",
                    "CourseCategoryImgUrlInputDesc": "Please input your CourseCategoryImgUrl",
                     
"ParentId": "ParentId",
"Children": "Children",
					                     
                    "CourseCategory": "CourseCategory",
                    "ManageCourseCategory": "ManageCourseCategory",
                    "QueryCourseCategory": "QueryCourseCategory",
                    "CreateCourseCategory": "CreateCourseCategory",
                    "EditCourseCategory": "EditCourseCategory",
                    "DeleteCourseCategory": "DeleteCourseCategory",
                    "BatchDeleteCourseCategory": "BatchDeleteCourseCategory",
                    "ExportCourseCategory": "ExportCourseCategory",
                    "CourseCategoryList": "CourseCategoryList",
                     //课程分类的多语言配置==End
                    




```
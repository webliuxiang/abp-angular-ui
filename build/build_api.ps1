## RESTORE NUGET PACKAGES #####################################################
## Write-Host  $rootFolder
## 删除原本的publish文件夹
## 采用 长路径删除命令。防止不过发生。。
##  Remove-PathToLongDirectory $fontOutputFolder -Force -Recurse -ErrorAction Ignore




## Write-Host  $outputFolder
Write-Host  "开始发布后台，发布目录为："$endOutputFolder
## echo 
## PUBLISH WEB HOST PROJECT ###################################################

## https://docs.microsoft.com/zh-cn/dotnet/core/tools/index?tabs=netcore2x 官方文档地址
## echo "开始发布后台，发布目录为："$endOutputFolder
Set-Location $webHostFolder


# 读取
$appSettingsJsonInput = (Get-Content -Raw -Path "appsettings.json" )
## Write-Host $appSettingsJsonInput
$appSettings = ConvertFrom-Json -InputObject $appSettingsJsonInput
## $appSettings = $appSettingsJsonInput | ConvertFrom-Json

# Server=localhost; Database=YoyoCmsTemplate_db_blogModuledd; Trusted_Connection=True;MultipleActiveResultSets=True
## 修改默认的链接字符串的配置内容

## 获取仓库中的连接字符串变量来覆盖本地的字符串内容
$appSettings.ConnectionStrings.Default = $env:stage_default
# 写入
$appSettingsJsonOutput = ConvertTo-Json  $appSettings
Set-Content -Encoding "utf8" -Path "appsettings.json" -Value $appSettingsJsonOutput




dotnet publish -c Release  --output $endOutputFolder
## echo "发布后台结束"
## https://docs.gitlab.com/ee/ci/merge_request_pipelines/

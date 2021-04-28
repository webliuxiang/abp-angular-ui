# COMMON PATHS
$buildFolder = (Get-Item -Path "./" -Verbose).FullName
$slnFolder = Join-Path $buildFolder "src/yoyocmstemplate-aspnet-core"
$webHostFolder = Join-Path $slnFolder "src/LTMCompanyName.YoyoCmsTemplate.Web.Host"
$ngFolder = Join-Path $buildFolder "src/yoyocmstemplate-angular"
Set-Location /
$rootFolder = (Get-Item -Path "./" -Verbose).FullName
$outputFolder = Join-Path $rootFolder "publish"
$endOutputFolder = Join-Path $outputFolder "Host"
$fontOutputFolder = (Join-Path $outputFolder "ng")
$MigrationFolder = Join-Path $slnFolder "tools/LTMCompanyName.YoyoCmsTemplate.Migrator"



## 打印各种变量

Write-Host "各种变量的打印输出"

Write-Host "buildFolder:" $buildFolder  
Write-Host "slnFolder:" $slnFolder

Write-Host "webHostFolder:" $webHostFolder

Write-Host "ngFolder:" $ngFolder

Write-Host "nodeModuleFolder:" $nodeModuleFolder

Write-Host "rootFolder:" $rootFolder

Write-Host "outputFolder:" $outputFolder

Write-Host "endOutputFolder:" $endOutputFolder

Write-Host "endOutputFolder:" $endOutputFolder
Write-Host "fontOutputFolder:" $fontOutputFolder
Write-Host "buildFolder:" $buildFolder


## RESTORE NUGET PACKAGES #####################################################


## 开发环境下：1 还原包

# 检查触发条件。


## 迁移功能链接字符串功能调整

# 1、读取配置中心中的连接字符串中的值
# 2、判断当前发布环境到正式环境还是staging环境。
# 3、替换当前appsettings.Production.json中的值，替换的变量来自仓库中的变量。
# 4、通过分支控制来保证功能触发的正常。

## 查看几个系统中的参数读取方式 http://code.52abp.com/help/ci/variables/README#variables

## echo "开始还原后台"
Set-Location $webHostFolder
# 读取
$appSettingsJsonInput = (Get-Content -Raw -Path "appsettings.json" )
## Write-Host $appSettingsJsonInput

$appSettings = $appSettingsJsonInput | ConvertFrom-Json

# $appSettings = ConvertFrom-Json -InputObject $appSettingsJsonInput
# Server=localhost; Database=YoyoCmsTemplate_db_blogModuledd; Trusted_Connection=True;MultipleActiveResultSets=True
$appSettings.ConnectionStrings.Default = "Server=localhost; Database=12333; Trusted_Connection=True;MultipleActiveResultSets=True"
# 写入
$appSettingsJsonOutput = ConvertTo-Json  $appSettings
Set-Content -Encoding "utf8" -Path "appsettings.json" -Value $appSettingsJsonOutput


## dotnet build

##Write-Host  $slnFolder

## dotnet restore
## echo "还原后台结束"
## PUBLISH WEB HOST PROJECT ###################################################
## echo "开始发布后台，发布目录为："$endOutputFolder
## Set-Location $webHostFolder
## dotnet publish --output $endOutputFolder
## echo "发布后台结束"
## PUBLISH ANGULAR UI PROJECT #################################################
## echo "开始发布前端，发布目录为："$fontOutputFolder
## Set-Location $ngFolder

##& yarn
## & yarn build
## Remove-Item $fontOutputFolder -Force -Recurse -ErrorAction Ignore
##Copy-Item (Join-Path $ngFolder "dist") $fontOutputFolder -Recurse
## echo "发布前端结束"
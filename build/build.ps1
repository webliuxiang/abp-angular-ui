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
## echo "开始还原后台"
## Set-Location $slnFolder
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
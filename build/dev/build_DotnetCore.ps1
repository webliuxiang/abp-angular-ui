
Write-Host "Build Just WebHost solution"
Write-Host  $slnFolder


## 开始编译后端

## RESTORE NUGET PACKAGES #####################################################
## echo "开始还原后台"
Set-Location $slnFolder
dotnet build

Write-Host "Complete WebHost host build "

 
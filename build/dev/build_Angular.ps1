Write-Host "Build Just angular solution"
Write-Host "Start build : " $ngFolder

## Write-Host  "开始发布ANGULAR前端，发布目录为："$endOutputFolder
Write-Host  "Start build Angular and Publish ："$endOutputFolder

# 
## 开始编译后端

## RESTORE NUGET PACKAGES #####################################################
## 设置路径
Set-Location $ngFolder
& yarn ## 前端包的还原
& yarn buildng
## echo "前端编译完成"


# 删除长路径
function Remove-PathToLongDirectory {
    Param(
        [string]$directory
    )

    # create a temporary (empty) directory
    $parent = [System.IO.Path]::GetTempPath()
    [string] $name = [System.Guid]::NewGuid()
    $tempDirectory = New-Item -ItemType Directory -Path (Join-Path $parent $name)

    robocopy /MIR $tempDirectory.FullName $directory | out-null
    Remove-Item $directory -Force | out-null
    Remove-Item $tempDirectory -Force | out-null
}
Remove-PathToLongDirectory $nodeModuleFolder


$ngdistFolder = Join-Path $ngFolder "dist"

echo $ngdistFolder

if (-not (Test-Path $ngdistFolder)) {
    # 如果dist文件夹不存在，说明编译失败，触发异常
    throw "The front-end compilation fails, triggering an exception"
} 

Write-Host "Build  angular finished"

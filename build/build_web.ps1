## PUBLISH ANGULAR UI PROJECT #################################################
echo "开始发布前端，发布目录为："$fontOutputFolder
## Write-Host  "开始发布ANGULAR前端，发布目录为："$endOutputFolder
Write-Host  "Start build Angular and Publish ："$endOutputFolder

Set-Location $ngFolder 
& yarn
## 编译staging环境变量信息
& yarn staging
##
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
## echo "发布前端结束"

$ngdistFolder = Join-Path $ngFolder "dist"

echo $ngdistFolder
echo "Check whether the front-end project is compiled properly"
if (-not (Test-Path $ngdistFolder)) {
    # 如果dist文件夹不存在，说明编译失败，触发异常
    throw "The front-end compilation fails, triggering an exception"
} 
## 编译通过后删除前端文件夹
Remove-Item $fontOutputFolder -Force -Recurse -ErrorAction Ignore ##删除Publish/ng的文件夹路径
Copy-Item (Join-Path $ngFolder "dist") $fontOutputFolder -Recurse # 复制ngfolder文件夹下的dist文件夹




# $fileNames = "a.txt", "b.ps1", "c.mp3"
# $result 
# $fileNames | foreach {
#     if (-not (Test-Path -Path "你的目录" -Include $_)) {
#         $result = $false;
#         break; 
#     }
# }
# #判断result 结果
# $result


# $filelist = Get-Content "file.txt" #获取要检查的文件列表
# $csvs = new-object collections.arraylist #创建一个arraylist对象
# foreach ($file in $filelist) {
#     $csv = new-psobject | select yes, no
#     if ([io.Directory]::Exists($file)) {
#         #判断文件是否存在
#         $csv.yes = $file
#     }
#     else {
#         $csv.no = $file
#     }
#     $null = $csvs.add($csv)
# }
# $csvs | Export-Csv file.csv -notype  -Encoding oem #导出成csv文件
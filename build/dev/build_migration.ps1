
## 进行数据库的迁移 
Set-Location $MigrationFolder
echo  $MigrationFolder

## 对appsettings.json文件中的参数进行处理
echo "Start Migration"


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

## 运行迁移命令， 
## dotnet run -q 

##  Write-Host "Complete database migration"


## echo "还原后台结束"

## 生成可独立部署的exe包

# dotnet publish -c Release --self-contained -r win-x86

## echo "Complete WebHost host build and database migration"

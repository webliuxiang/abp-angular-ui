# COMMON PATHS
$buildFolder = (Get-Item -Path "./" -Verbose).FullName
$vueFolder = Join-Path $buildFolder "src/yoyocmstemplate-vue"


Set-Location $vueFolder 

docker-compose up -d
# COMMON PATHS
$buildFolder = (Get-Item -Path "./" -Verbose).FullName
$ngFolder = Join-Path $buildFolder "src/yoyocmstemplate-angular"


Set-Location $ngFolder 

docker-compose up -d
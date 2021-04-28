# COMMON PATHS
Write-Host "Start Run powerShell"

Write-Host "Setting All Folders"
Write-Host "Build all solutions"

$buildFolder = (Get-Item -Path "./" -Verbose).FullName
$slnFolder = Join-Path $buildFolder "../"
$outputFolder = Join-Path $buildFolder "outputs"
$webHostFolder = Join-Path $slnFolder "src/LTMCompanyName.YoyoCmsTemplate.Web.Host"
$ngFolder = Join-Path $buildFolder "../../yoyocmstemplate-angular"

Write-Host "ngFolder"


## CLEAR ######################################################################
Write-Host "Delete Temp Folder"
Remove-Item $outputFolder -Force -Recurse -ErrorAction Ignore
New-Item -Path $outputFolder -ItemType Directory

## RESTORE NUGET PACKAGES #####################################################

# Write-Host "build Release"
# Set-Location $slnFolder
# dotnet build --configuration Release

## PUBLISH WEB HOST PROJECT ###################################################

Set-Location $webHostFolder
dotnet publish --output (Join-Path $outputFolder "Host")

Write-Host "dotnet Publish Success ....."


## PUBLISH ANGULAR UI PROJECT #################################################
Write-Host "Start Build Angular UI ....."

Set-Location $ngFolder
& yarn
& yarn build
Copy-Item (Join-Path $ngFolder "dist") (Join-Path $outputFolder "ng") -Recurse
Copy-Item (Join-Path $ngFolder "Dockerfile") (Join-Path $outputFolder "ng")

# Change UI configuration
$ngConfigPath = Join-Path $outputFolder "ng/assets/appconfig.prod.json"
(Get-Content $ngConfigPath) -replace "21021", "9901" | Set-Content $ngConfigPath
(Get-Content $ngConfigPath) -replace "4200", "9902" | Set-Content $ngConfigPath

## CREATE DOCKER IMAGES #######################################################

# Host
Set-Location (Join-Path $outputFolder "Host")

docker rmi abp/host -f
docker build -t abp/host .

# Angular UI
Set-Location (Join-Path $outputFolder "ng")

docker rmi abp/ng -f
docker build -t abp/ng .

## DOCKER COMPOSE FILES #######################################################

Copy-Item (Join-Path $slnFolder "docker/ng/*.*") $outputFolder

## FINALIZE ###################################################################

Set-Location $outputFolder
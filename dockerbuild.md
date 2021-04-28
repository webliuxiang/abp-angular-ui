# 使用 Docker-compose 编译项目

| 名称                       | 作用                              |
| -------------------------- | --------------------------------- |
| docker-compose-prod.yml    | 生产环境使用                      |
| docker-compose-staging.yml | 演示环境使用                      |
| docker-compose-dev.yml     | 开发环境使用                      |
| docker-compose.yml         | 本地运行使用 docker-compose build |

- docker-compose-prod.yml
- docker-compose-staging.yml
- docker-compose.yml
- docker-compose-dev.yml

## Docker 编译命令调试

导航到目录`52abp-Enterprise`文件夹后

```
cd C:\Code\gitlab\52abp-Enterprise
```

```
# host
docker build . --force-rm  -t 52abp_pro_host_demo  -f .\src\yoyocmstemplate-aspnet-core\src\LTMCompanyName.YoyoCmsTemplate.Web.Host\Dockerfile.host.compose

# angular
docker build . --force-rm  -t 52abp_pro_ng_demo  -f .\src\yoyocmstemplate-angular\Dockerfile.ng.compose

# Migrator 迁移工具

docker build . --force-rm  -t 52abp_pro_migrator_demo  -f .\src\yoyocmstemplate-aspnet-core\tools\LTMCompanyName.YoyoCmsTemplate.Migrator\Dockerfile.migrator.compose

```

### 创建 demo

```bash

docker run   -d --name 52abp_pro_host_8022   -p 8022:80  52abp_pro_host_demo:latest

docker run -d  --name   52abp_pro_ng_8023  -p 8023:80  52abp_pro_ng_demo:latest

docker run -d --name 52abp_pro_migrator_8011    -p 8011:80  52abp_pro_migrator_demo:latest
```

## docker-compose 编译并且运行

导航到目录`52abp-Enterprise`文件夹后

```bash
docker-compose build # 编译镜像

docker-compose up -d # 启动镜像

```

## 手动发布到阿里云

```bash
docker tag 52abp-enterprise_52abp_pro_host registry.cn-hangzhou.aliyuncs.com/yoyosoft/52abp_pro-host

docker tag 52abp-enterprise_52abp_pro_migrator registry.cn-hangzhou.aliyuncs.com/yoyosoft/52abp_pro-migrator

docker tag 52abp-enterprise_52abp_pro_ng registry.cn-hangzhou.aliyuncs.com/yoyosoft/52abp_pro-ng

docker tag 52abp-enterprise_52abp_pro_vue registry.cn-hangzhou.aliyuncs.com/yoyosoft/52abp_pro-vue

### 发布到阿里云仓库



docker push registry.cn-hangzhou.aliyuncs.com/yoyosoft/52abp_pro-host

docker push registry.cn-hangzhou.aliyuncs.com/yoyosoft/52abp_pro-migrator

docker push registry.cn-hangzhou.aliyuncs.com/yoyosoft/52abp_pro-ng

docker push registry.cn-hangzhou.aliyuncs.com/yoyosoft/52abp_pro-vue


```

### 移除悬虚镜像

```bash
docker image prune
```

## Linux 下测试

```bash
docker build . --force-rm  -t abpng_demo -f src/yoyocmstemplate-angular/Dockerfile.copy
```

## powershel 命令

```powershell

Remove-Item .\vsts\ -Force -Recurse -ErrorAction Ignore
```

## 添加

```bash
npm install wait-for-it.sh@1.0.0
```

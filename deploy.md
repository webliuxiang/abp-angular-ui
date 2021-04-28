# 部署 52abp 前后端分离的 docker 镜像到容器中

打开`docker-deploy-azure.yml`，同时需要保证`docker`文件夹也存在

采用命令启动镜像

```bash

docker-compose -f docker-deploy-azure.yml -p yoyo up -d

docker-compose -f docker-deploy-azure.yml -p yoyo ps

docker logs -f yoyo_52abp_pro_migrator_1
 docker stop  yoyo_52abp_pro_migrator_1

```

## 删除容器

```
docker-compose -f docker-deploy-azure.yml -p yoyo down
```

```
# 删除容器包含数据卷

docker-compose -f docker-deploy-azure.yml -p yoyo down -v

```

## 监控容器状态

```bash
docker stats
```

## 临时环境验证的命令

```bash

docker exec -it  yoyo_52abp_pro_host_1 bash

```

```bash
rm appsettings.Production.json
```

docker restart yoyo_52abp_pro_host_1





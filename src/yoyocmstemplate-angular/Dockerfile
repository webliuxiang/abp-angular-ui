# STEP 1: Build
FROM registry.cn-hangzhou.aliyuncs.com/yoyosoft/node:13.2-alpine as builder

LABEL authors="ltm <ltm@ddxc.org>"

COPY src/WebManagement-angular/package.json src/WebManagement-angular/package-lock.json ./

RUN npm set progress=false && npm config set depth 0 && npm cache clean --force
RUN npm i && mkdir /ng-alain && cp -R ./node_modules ./ng-alain

WORKDIR /ng-alain

COPY . .

RUN npm run build
# 测试docker脚本。
# STEP 2: Setup
FROM registry.cn-hangzhou.aliyuncs.com/yoyosoft/nginx:1.17.6-alpine

COPY --from=builder /ng-alain/_nginx/default.conf /etc/nginx/conf.d/default.conf
# COPY --from=builder /ng-alain/_nginx/ssl/ /etc/nginx/ssl/

RUN rm -rf /usr/share/nginx/html/*

COPY --from=builder /ng-alain/dist /usr/share/nginx/html

CMD [ "nginx", "-g", "daemon off;"]

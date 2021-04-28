# STEP 1: Build
FROM node:13.2-alpine as builder

LABEL authors="ltm <ltm@ddxc.org>"

COPY package.json package-lock.json ./

RUN npm set progress=false && npm config set depth 0 && npm cache clean --force
RUN npm i && mkdir /vue && cp -R ./node_modules ./vue


WORKDIR /vue

COPY . .

RUN npm run build
# ≤‚ ‘dockerΩ≈±æ°£
# STEP 2: Setup
FROM nginx:1.17.6-alpine

COPY --from=builder /vue/_nginx/default.conf /etc/nginx/conf.d/default.conf
# COPY --from=builder /vue/_nginx/ssl/ /etc/nginx/ssl/

RUN rm -rf /usr/share/nginx/html/*

COPY --from=builder /vue/dist /usr/share/nginx/html

CMD [ "nginx", "-g", "daemon off;"]

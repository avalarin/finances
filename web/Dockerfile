FROM node:7.0.0

MAINTAINER avalarin@gmail.com

RUN apt-get update
RUN apt-get install -y apt-transport-https curl

ENV wd /opt/finances-web
WORKDIR ${wd}

ADD package.json ${wd}/package.json

RUN npm install

ADD . ${wd}

EXPOSE 8080
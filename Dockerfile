FROM ubuntu:16.04

MAINTAINER avalarin@gmail.com

RUN apt-get update
RUN apt-get install -y apt-transport-https

# Install dotnet core
RUN sh -c 'echo "deb [arch=amd64] https://apt-mo.trafficmanager.net/repos/dotnet-release/ xenial main" > /etc/apt/sources.list.d/dotnetdev.list'
RUN apt-key adv --keyserver apt-mo.trafficmanager.net --recv-keys 417A0893
RUN apt-get update
RUN apt-get install -y dotnet-dev-1.0.0-preview2.1-003155

ENV wd /opt/finances
WORKDIR ${wd}

ADD src/Finances/project.json ${wd}/src/Finances/project.json
ADD src/Finances/project.lock.json ${wd}/src/Finances/project.lock.json
ADD test/Finances.Test/project.json ${wd}/test/Finances.Test/project.json
ADD test/Finances.Test/project.lock.json ${wd}/test/Finances.Test/project.lock.json

RUN dotnet restore

ADD . ${wd}

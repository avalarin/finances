# Finances back-end

## Requirements
* dotnet core https://www.microsoft.com/net/core
* postgresql server

## How to launch
```bash
dotnet restore
dotnet run -p src/Finances/project.json
# or
dotnet watch run -p src/Finances/project.json
```

## How to launch with docker

1. Install docker and docker-compose
1. Execute `docker-compose up`
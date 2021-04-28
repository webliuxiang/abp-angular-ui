#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# mcr.microsoft.com/dotnet/core/aspnet:3.1
# mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim
# mcr.microsoft.com/dotnet/core/sdk:3.1
FROM registry.cn-hangzhou.aliyuncs.com/yoyosoft/dotnet/core/aspnet:3.1 AS base
WORKDIR /app

FROM registry.cn-hangzhou.aliyuncs.com/yoyosoft/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["tools/LTMCompanyName.YoyoCmsTemplate.Migrator/LTMCompanyName.YoyoCmsTemplate.Migrator.csproj", "tools/LTMCompanyName.YoyoCmsTemplate.Migrator/"]
COPY ["src/LTMCompanyName.YoyoCmsTemplate.Core/LTMCompanyName.YoyoCmsTemplate.Core.csproj", "src/LTMCompanyName.YoyoCmsTemplate.Core/"]
COPY ["src/LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore/LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore.csproj", "src/LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore/"]
RUN dotnet restore "tools/LTMCompanyName.YoyoCmsTemplate.Migrator/LTMCompanyName.YoyoCmsTemplate.Migrator.csproj"
COPY . .
WORKDIR "/src/tools/LTMCompanyName.YoyoCmsTemplate.Migrator"
RUN dotnet build "LTMCompanyName.YoyoCmsTemplate.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LTMCompanyName.YoyoCmsTemplate.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LTMCompanyName.YoyoCmsTemplate.Migrator.dll"]
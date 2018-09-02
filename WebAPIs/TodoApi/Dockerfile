FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY TodoApi/TodoApi.csproj TodoApi/
RUN dotnet restore TodoApi/TodoApi.csproj
COPY . .
WORKDIR /src/TodoApi
RUN dotnet build TodoApi.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish TodoApi.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TodoApi.dll"]

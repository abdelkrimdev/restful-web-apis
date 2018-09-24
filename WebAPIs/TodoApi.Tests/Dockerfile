FROM microsoft/dotnet:2.1-sdk
WORKDIR /src
COPY . .

WORKDIR /src/TodoApi.Tests
RUN dotnet restore
ENTRYPOINT dotnet test --logger trx

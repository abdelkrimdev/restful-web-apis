FROM microsoft/dotnet:2.1-sdk
WORKDIR /src
COPY . .

WORKDIR /src/Shared.Tests
RUN dotnet restore
ENTRYPOINT dotnet test --logger trx

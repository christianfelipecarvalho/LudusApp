# Usar a imagem base do .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Definir o diretório de trabalho
WORKDIR /app

# Copiar o arquivo de solução .sln para o contêiner
COPY LudusApp.sln ./

# Copiar os arquivos .csproj das respectivas pastas
COPY LudusApp/*.csproj LudusApp/
COPY LudusApp.Application/*.csproj LudusApp.Application/
COPY LudusApp.Domain/*.csproj LudusApp.Domain/
COPY LudusApp.Infra.Data/*.csproj LudusApp.Infra.Data/

# Restaurar pacotes NuGet
RUN dotnet restore LudusApp.sln

# Copiar o restante do código
COPY . .

# Publicar o aplicativo
RUN dotnet publish LudusApp/LudusApp.csproj -c Release -o /app/publish

# Definir a imagem de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Definir o diretório de trabalho no contêiner
WORKDIR /app

# Copiar os arquivos publicados para o contêiner
COPY --from=build /app/publish .

# Definir o comando para rodar o aplicativo
ENTRYPOINT ["dotnet", "LudusApp.dll"]

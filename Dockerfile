# Usar a imagem base do .NET SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Definir o diretório de trabalho
WORKDIR /app

# Copiar e restaurar dependências
COPY *.sln ./
COPY LudusApp/*.csproj LudusApp/
COPY LudusApp.Application/*.csproj LudusApp.Application/
COPY LudusApp.Domain/*.csproj LudusApp.Domain/
COPY LudusApp.Infra.Data/*.csproj LudusApp.Infra.Data/
RUN dotnet restore

# Copiar o restante do código e compilar
COPY . .
RUN dotnet publish LudusApp/LudusApp.csproj -c Release -o /app/publish

# Usar a imagem otimizada de runtime do .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

# Copiar arquivos publicados
COPY --from=build /app/publish .

# Expor a porta 80 para comunicação interna
EXPOSE 80

# Definir o comando de entrada
ENTRYPOINT ["dotnet", "LudusApp.dll"]

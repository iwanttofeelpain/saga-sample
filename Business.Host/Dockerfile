FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Business.Host/Business.Host.csproj", "Business.Host/"]
RUN dotnet restore "Business.Host/Business.Host.csproj"
COPY . .
WORKDIR "/src/Business.Host"
RUN dotnet build "Business.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Business.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Business.Host.dll"]

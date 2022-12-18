#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["8obcy/8obcy.csproj", "8obcy/"]
RUN dotnet restore "8obcy/8obcy.csproj"
COPY . .
WORKDIR "/src/8obcy"
RUN dotnet build "8obcy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "8obcy.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "8obcy.dll"]
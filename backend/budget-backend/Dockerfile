#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["budget-backend/budget-backend.csproj", "budget-backend/"]
RUN dotnet restore "budget-backend/budget-backend.csproj"
COPY . .
WORKDIR "/src/budget-backend"
RUN dotnet build "budget-backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "budget-backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "budget-backend.dll"]
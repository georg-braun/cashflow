FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["cashflow/cashflow.csproj", "cashflow/"]
RUN dotnet restore "cashflow/cashflow.csproj"
COPY . .

RUN dotnet test

WORKDIR "/src/cashflow"
RUN dotnet build "cashflow.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cashflow.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cashflow.dll"]
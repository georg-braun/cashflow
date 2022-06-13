# cash flow 💰

Log and analyze income and spendings.

Software for managing personal finance with budgets.

# Architecture

Web app (sveltekit) that authenticates with Auth0 and interact with a API (ASP.NET) that store the data in a postgresql database.

![](./system-overview.excalidraw.png)


# Setup

## Requirements
- Auth0 account with configured API and Application
- Container environment (e.g. docker)


## Setup database

Add necessary configuration variables


1. run postgresql container 
    ```bash
    cd cashflow-services/local-postgres
    docker-compose up -d
    ```
2. create and migrate database
   ```bash
   ./utility-cli migrate "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres"  "~/Repositories/budget/cashflow-cli/Scripts"
   ```
   
## Get the API running

### Local Development
1. Go to the `/backend/cashflow` folder 
2. Add necessary configuration for the ASP.NET API in `appsettings.json`
   - `ConnectionStrings:Database`
   - `Auth0:Domain`
   - `Auth0:Audience`

It's also possible to add these as environment variables.

Example: Environment variables via secret manager
1. Navigate to backend folder
2. `dotnet user-secrets set "ConnectionStrings:Database" "<connection-string>"`

### Container
You can use the `/backend/docker-compose.yml` to build and run the API. But you have to comment out the build step and insert the necessary environment variables.


## Get the frontend running

1. Go  to the `/frontend` folder
2. Add necessary configuration via environment variables (global or via `.env` file)
   - `VITE_AUTH_CLIENT_ID=<AUTH0_DASHBOARD>`
   - `VITE_AUTH_DOMAIN=<AUTH0_DASHBOARD>`
   - `VITE_BUDGET_API_SERVER=<CASHFLOW-API-ADDRESS>`


# Backup database

The easiest (manual) way to do this is using `pg_dump`.

`pg_dump postgres://user:password@host:5432/database > backup-file`

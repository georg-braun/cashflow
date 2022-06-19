# cash flow 💰

Analyze your spendings.

I built this app to track my spendings and create the features I need.

# Features
- Associate income/spending with a category
- Show the monthly spending per category
- Use templates to simplify recurring events. Manual trigger

## Roadmap
- Sankey diagram per month
- Automated template trigger per day of month
- Automated template trigger per day of year


# Architecture

Web app (sveltekit) that authenticates with Auth0 and interact with a API (ASP.NET) that store the data in a postgresql database.

![](./system-overview.excalidraw.png)


# Setup

## Requirements
- Auth0 account with configured API and Application

## 1. Setup PostgreSQL database

Add necessary configuration variables


- [ ] run postgresql container 
    ```bash
    cd cashflow-services/local-postgres
    docker-compose up -d
    ```
- [ ] create and migrate database
   ```bash
   ./utility-cli migrate "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres"  "~/Repositories/budget/cashflow-cli/Scripts"
   ```
   
## 2. Get the API running

```
// Environment variables
ConnectionStrings:Database
Auth0:Domain
Auth0:Audience
```

### Local Development
1. Go to the `/backend/cashflow` folder 
2. Add necessary environment variables
   - Option 1: `appsettings.json`
   - Option 2: `dotnet user-secrets set "ConnectionStrings:Database" "<connection-string>"`

### Container
You can use the `/backend/docker-compose.yml` to build and run the API. But you have to comment out the build step and insert the necessary environment variables.


## 3. Get the frontend running

```
// Environment variables
VITE_AUTH_CLIENT_ID=<AUTH0_DASHBOARD>
VITE_AUTH_DOMAIN=<AUTH0_DASHBOARD>
VITE_BUDGET_API_SERVER=<CASHFLOW-API-ADDRESS>
```

1. Go  to the `/frontend` folder
2. Add necessary configuration via environment variables (global or via `.env` file)
   


# Backup database

The easiest (manual) way to do this is using `pg_dump`.

- locally installed
  
   `pg_dump postgres://user:password@host:5432/database > backup-file.sql`

- via docker container
  
   `docker run -i postgres /usr/bin/pg_dump postgres://user:password@host:5432/database > backup-file.sql`

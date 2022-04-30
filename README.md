# cash flow 💰

Log and analyze income and spendings.

Software for managing personal finance with budgets.

# Architecture

Web app (sveltekit) that authenticates with Auth0 and interact with a API (ASP.NET) that store the data in a postgresql database.


# Setup development environment


## Backend

Add necessary configuration variables
```
ConnectionStrings:Database
Auth0:Domain
Auth0:Audience
```
In docker `__` as delimiter: `ConnectionString__Database`

Example: Environment variables via secret manager

1. Navigate to backend folder
2. `dotnet user-secrets set "ConnectionStrings:Database" "<connection-string>"`


## Database migration

In `/utility-cli` you can find a cli tool that help to create and provision the database.



# CI/CD

# Production details

## Docker

`docker run -d -p 8001:80 --name budget-backend budget-backend`

 access via http://localhost:8001/...

## local minikube

1. create Kubernetes deployment + service of the backend`kubectl apply -f backend-deployment.yaml`
2. access service via NodePort services `minikube service --url backend-api-service`

*background: https://minikube.sigs.k8s.io/docs/handbook/accessing/*

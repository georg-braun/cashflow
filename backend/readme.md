# Notes

# setup development machine

**Persistence**

- Add database connection string environment variable
  ` dotnet user-secrets set "ConnectionStrings:Database" "<connection-string>"`
- Generate database structure
    - use `dotnet ef` migration / sql

# Build for deployment

## Docker

Run image

docker run -d -p 8001:80 --name budget-backend budget-backend access via http://localhost:8001/...

## local minikube

1. create Kubernetes deployment + service of the backend`kubectl apply -f backend-deployment.yaml`
2. access service via NodePort services `minikube service --url backend-api-service`

*background: https://minikube.sigs.k8s.io/docs/handbook/accessing/*



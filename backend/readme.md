# Notes

# Conitnuous Deployment

Main branch changed
--> Github Action: build image
--> Github Action: publish image to image repository

# Build

## Docker

Run image

 docker run -d -p 8001:80 --name budget-backend budget-backend
 access via http://localhost:8001/...

 ## local minikube

1. create Kubernetes deployment + service of the backend`kubectl apply -f backend-deployment.yaml` 
2. access service via NodePort services `minikube service --url backend-api-service`


*background: https://minikube.sigs.k8s.io/docs/handbook/accessing/*





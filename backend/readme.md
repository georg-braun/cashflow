# Notes

# Deployment

Main branch changed
--> Github Action: build image
--> Github Action: publish image to image repository

# Docker

Run image

 docker run -d -p 8001:80 --name my-test-budget-backend budget-backend
 access via http://localhost:8001/...
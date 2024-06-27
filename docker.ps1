docker build --no-cache -t my-api-image .
docker run -d -p 9091:9091 --name my-api-container my-api-image

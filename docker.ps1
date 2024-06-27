docker build --no-cache -t doc-planner-challenge .
docker run -d -p 9091:9091 --name doc-planner-challenge-container doc-planner-challenge

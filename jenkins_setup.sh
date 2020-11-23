#!/usr/bin/env bash

set -xe

JENKINS_PORT_ON_LOCALHOST=9090
JENKINS_DOCKER_IMAGE_TAG=local_jenkins_with_docker
JENKINS_CONTAINER_NAME=jenkins_local

# build local jenkins docker image with docker installed inside. 
docker build -t $JENKINS_DOCKER_IMAGE_TAG -f ./local_jenkins_with_docker.dockerfile .

# create volume to mount jenkins home to persist your jenkins data across multiple runs of container
docker volume create jenkins_home

# run your jenkins container with custom image that you created above.
docker run -d \
  -p $JENKINS_PORT_ON_LOCALHOST:8080 \
  -v jenkins_home:/var/jenkins_home \
  -v /var/run/docker.sock:/var/run/docker.sock \
  --name $JENKINS_CONTAINER_NAME \
  $JENKINS_DOCKER_IMAGE_TAG

# get your initial admin password. You need that when you run jenkins for the first time.
echo "Use this password if you are setting up your jenkins for the first time: "
docker exec $JENKINS_CONTAINER_NAME cat /var/jenkins_home/secrets/initialAdminPassword


echo "Done with running jenkins inside a docker container. Use link http://localhost:$JENKINS_PORT_ON_LOCALHOST/ to visit jenkins via browser"

##### References: 
# https://tutorials.releaseworksacademy.com/learn/the-simple-way-to-run-docker-in-docker-for-ci
# https://stackoverflow.com/questions/10498554/jenkins-linking-to-my-local-git-repository
# https://rangle.io/blog/running-jenkins-and-persisting-state-locally-using-docker-2/
#####


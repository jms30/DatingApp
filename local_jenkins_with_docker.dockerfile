FROM jenkins/jenkins:lts

USER root

RUN apt-get update && \
   apt-get -y install apt-transport-https \
   ca-certificates \
   curl \
   gnupg2 \
   software-properties-common && \
   curl -fsSL https://download.docker.com/linux/$(. /etc/os-release; echo "$ID")/gpg > /tmp/dkey; apt-key add /tmp/dkey && \
   add-apt-repository \
   "deb [arch=amd64] https://download.docker.com/linux/$(. /etc/os-release; echo "$ID") \
   $(lsb_release -cs) \
   stable" && \
   apt-get update && \
   apt-get -y install docker-ce

# need to add jenkins user to docker group and root group so that docker commands in jenkins file do not get permission errors. 
# running docker plugin in jenkins basically forwards to docker daemon running on your host box.
RUN usermod -aG docker jenkins
RUN usermod -aG root jenkins
RUN newgrp docker

USER jenkins

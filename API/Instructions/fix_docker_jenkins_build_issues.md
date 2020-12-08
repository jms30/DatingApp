1. This is to fix when there's a mismatch between your `jenkins` user id on local container and local host system. If for some reason, you see a mismatch. you first need to fix this by modifying the original user id on the local container with some new, non-existing user id and then modify the same on local host system. To do so, log in as root on jenkins container and then use
    ```bash
    $ docker exec -it --user=root jenkins_local /bin/bash
    $ usermod -u <new non conflicting user id> jenkins
    ```
    Confirm that the change has happened with following command:
    ```bash
    $ id -u jenkins
    ```
2. Afterwards, check if `jenkins` user exists on local system. If it does, then you also have to fix there with Step 1.
3. On local host system, check the `docker` group id using command: 
    ```bash
    $ cat /etc/group | grep docker
    ```
4. Now you have `docker` group id and `jenkins` user id. Last step is to assign the `docker` group id to `jenkins` user. To do, use following command on local system(if the `jenkins` user exists on local system) as well as inside jenkins container:
    ```bash
    $ usermod -aG <docker group id> jenkins
    ```
5. Confirm that `jenkins` user is added to `docker` group with following command on jenkins container:
    ```bash
    $ id jenkins 
    ```
6. Confirm that your current user is also attached to the docker group on local system:
    ```bash
    $ id $USER
    ```
7. Both of the previous 2 steps should have `docker` common in the group along with the respective Group Id.